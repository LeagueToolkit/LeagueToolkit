using System;
using System.Linq;
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
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[GitHubActions(
    "build",
    GitHubActionsImage.WindowsLatest,
    EnableGitHubToken = true,
    FetchDepth = 0,
    OnPushBranches = new[] { "main" },
    OnPullRequestBranches = new[] { "main" },
    ImportSecrets = new[] { nameof(NuGetApiKey) },
    InvokedTargets = new[] { nameof(Pack) }
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

    AbsolutePath SourceDirectory => RootDirectory / "src";

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        _ =>
            _.Description("Clean")
                .Before(Restore)
                .Executes(() =>
                {
                    DotNetTasks.DotNetClean(s => s.SetProject(Solution));

                    EnsureCleanDirectory(ArtifactsDirectory);
                });

    Target Restore =>
        _ =>
            _.Description("Restore")
                .DependsOn(Clean)
                .Executes(() =>
                {
                    DotNetTasks.DotNetRestore(s => s.SetProjectFile(Solution));
                });

    Target Compile =>
        _ =>
            _.Description("Build")
                .DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetTasks.DotNetBuild(
                        s =>
                            s.SetProjectFile(Solution)
                                .SetConfiguration(Configuration)
                                .SetVersion(MinVer.Version)
                                .EnableNoRestore()
                    );
                });

    Target Test =>
        _ =>
            _.DependsOn(Compile)
                .Executes(() =>
                {
                    DotNetTasks.DotNetTest(
                        s =>
                            s.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore().EnableNoBuild()
                    );
                });

    Target Pack =>
        _ =>
            _.DependsOn(Test)
                .Produces(ArtifactsDirectory / "*.nupkg")
                .Executes(() =>
                {
                    DotNetTasks.DotNetPack(
                        s =>
                            s.SetProject(Solution)
                                .SetOutputDirectory(ArtifactsDirectory)
                                .SetConfiguration(Configuration)
                                .EnableNoRestore()
                                .EnableNoBuild()
                                .SetVersion(MinVer.Version)
                    );
                });
}
