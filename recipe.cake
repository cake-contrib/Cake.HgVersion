#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "Cake.HgVersion",
    repositoryOwner: "cake-contrib",
    repositoryName: "Cake.HgVersion",
    appVeyorAccountName: "cakecontrib",
    solutionFilePath: "./src/Cake.HgVersion.sln",
    shouldRunCodecov: false,
    shouldRunDotNetCorePack: true,
    shouldRunIntegrationTests: true,
    wyamSourceFiles: "../../src/**/{!bin,!obj,!packages,!*Tests,}/**/*.cs",
    shouldRunGitVersion: true);

BuildParameters.PrintParameters(Context);
ToolSettings.SetToolSettings(context: Context);

Task("Unzip-Addin")
    .IsDependentOn("Package")
    .Does(() =>
{
    var addin = BuildParameters.Title;
    var semVersion = BuildParameters.Version.SemVersion;
    var nugetRoot = BuildParameters.Paths.Directories.NuGetPackages;
    var package = $"{nugetRoot}/{addin}.{semVersion}.nupkg";
    var addinDir = MakeAbsolute(Directory($"./tools/Addins/{addin}/{addin}"));

    if (DirectoryExists(addinDir))
    {
        DeleteDirectory(addinDir, new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        });
    }

    Unzip(package, addinDir);
});

BuildParameters.Tasks.IntegrationTestTask.IsDependentOn("Unzip-Addin");
BuildParameters.Tasks.PublishMyGetPackagesTask.IsDependentOn("Run-Integration-Tests");
BuildParameters.Tasks.PublishNuGetPackagesTask.IsDependentOn("Run-Integration-Tests");

Build.RunDotNetCore();
