using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitReleaseManager;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Utilities.Collections;
using Octokit;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions(
    "build",
    GitHubActionsImage.WindowsLatest,
    EnableGitHubToken = true,
    FetchDepth = 0,
    OnPushBranches = new[] { "main" },
    OnPullRequestBranches = new[] { "main" },
    OnPushTags = new[] { "main" },
    ImportSecrets = new[] { nameof(NuGetApiKey) },
    InvokedTargets = new[] { nameof(Clean) },
    OnPushIncludePaths = new[] { "src/**", "!src/build/**" }
)]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter]
    [Secret]
    readonly string NuGetApiKey;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [MinVer]
    readonly MinVer MinVer;

    [GitRepository]
    readonly GitRepository GitRepository;

    GitHubActions GitHubActions => GitHubActions.Instance;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        _ =>
            _.Description("Clean")
                .Before(Restore)
                .Executes(() =>
                {
                    DotNetClean(s => s.SetProject(Solution));

                    EnsureCleanDirectory(ArtifactsDirectory);
                });

    Target Restore =>
        _ =>
            _.Description("Restore")
                .DependsOn(Clean)
                .Executes(() =>
                {
                    DotNetRestore(s => s.SetProjectFile(Solution));
                });

    Target Compile =>
        _ =>
            _.Description("Build")
                .DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetBuild(
                        s =>
                            s.SetProjectFile(Solution)
                                .SetConfiguration(Configuration)
                                .SetVersion(MinVer.Version)
                                .SetAssemblyVersion(MinVer.AssemblyVersion)
                                .SetFileVersion(MinVer.FileVersion)
                                .EnableNoRestore()
                    );
                });

    Target Test =>
        _ =>
            _.DependsOn(Compile)
                .Executes(() =>
                {
                    DotNetTest(
                        s =>
                            s.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore().EnableNoBuild()
                    );
                });

    Target Pack =>
        _ =>
            _.DependsOn(Test)
                .Requires(() => Configuration.Equals(Configuration.Release))
                .Produces(ArtifactsDirectory / "*.nupkg")
                .Executes(() =>
                {
                    Log.Information(string.Join(',', GitRepository.Tags));
                    DotNetPack(
                        s =>
                            s.SetProject(Solution)
                                .SetOutputDirectory(ArtifactsDirectory)
                                .SetConfiguration(Configuration)
                                .EnableNoRestore()
                                .EnableNoBuild()
                                .SetVersion(MinVer.Version)
                                .SetAssemblyVersion(MinVer.AssemblyVersion)
                                .SetFileVersion(MinVer.FileVersion)
                    );
                });

    Target Release =>
        _ =>
            _.DependsOn(Pack)
                .Description("Release")
                .Requires(() => Configuration.Equals(Configuration.Release))
                .OnlyWhenStatic(() => GitRepository.IsOnMainBranch())
                .Triggers(PublishToNuget)
                .Executes(async () =>
                {
                    GitHubTasks.GitHubClient = new(new ProductHeaderValue(nameof(NukeBuild)))
                    {
                        Credentials = new Credentials(GitHubActions.Token)
                    };

                    NewRelease newRelease =
                        new(MinVer.Version)
                        {
                            TargetCommitish = GitHubActions.Sha,
                            Draft = true,
                            Name = MinVer.Version,
                            Prerelease = !string.IsNullOrEmpty(MinVer.MinVerPreRelease)
                        };

                    string owner = GitRepository.GetGitHubOwner();
                    string name = GitRepository.GetGitHubName();

                    Release createdRelease = await GitHubTasks.GitHubClient.Repository.Release.Create(
                        owner,
                        name,
                        newRelease
                    );

                    GlobFiles(ArtifactsDirectory, "*.nupkg")
                        .Where(x => !x.Contains("LeagueToolkit.Meta.Classes"))
                        .ForEach(async x => await UploadReleaseAssetToGithub(createdRelease, x));

                    await GitHubTasks.GitHubClient.Repository.Release.Edit(
                        owner,
                        name,
                        createdRelease.Id,
                        new ReleaseUpdate { Draft = false }
                    );
                });

    Target PublishToNuget =>
        _ =>
            _.Description("Publish to Nuget")
                .Requires(() => Configuration.Equals(Configuration.Release))
                .OnlyWhenStatic(() => GitRepository.IsOnMainBranch())
                .Executes(() =>
                {
                    GlobFiles(ArtifactsDirectory, "*.nupkg")
                        .Where(x => !x.Contains("LeagueToolkit.Meta.Classes"))
                        .ForEach(x =>
                        {
                            DotNetNuGetPush(s => s.SetTargetPath(x).SetApiKey(NuGetApiKey).EnableSkipDuplicate());
                        });
                });

    private static async Task UploadReleaseAssetToGithub(Release release, string asset)
    {
        string assetFileName = Path.GetFileName(asset);

        ReleaseAssetUpload assetUpload =
            new()
            {
                FileName = assetFileName,
                ContentType = "application/octet-stream",
                RawData = File.OpenRead(asset),
            };

        await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, assetUpload);
    }
}
