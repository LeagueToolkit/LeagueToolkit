using System;
using System.Linq;
using Microsoft.Build.Construction;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    [GitRepository]
    readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean =>
        _ =>
            _.Before(Restore)
                .Executes(() =>
                {
                    SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);

                    EnsureCleanDirectory(ArtifactsDirectory);
                });

    Target Restore =>
        _ =>
            _.Executes(() =>
            {
                DotNetTasks.DotNetRestore(s => s.SetProjectFile(Solution));
            });

    Target Compile =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetTasks.DotNetBuild(
                        s => s.SetProjectFile(Solution).SetConfiguration(Configuration).EnableNoRestore()
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
            _.DependsOn(Compile)
                .Executes(() =>
                {
                    DotNetTasks.DotNetPack(
                        s =>
                            s.SetProject(Solution)
                                .SetOutputDirectory(ArtifactsDirectory)
                                .SetIncludeSymbols(true)
                                .SetConfiguration(Configuration)
                                .EnableNoRestore()
                                .EnableNoBuild()
                    );
                });
}
