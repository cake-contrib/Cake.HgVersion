#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.HgVersion",
                            repositoryOwner: "vCipher",
                            repositoryName: "Cake.HgVersion",
                            appVeyorAccountName: "vCipher",
                            solutionFilePath: "./src/Cake.HgVersion.sln");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                                BuildParameters.RootDirectoryPath + "/src/Cake.HgVersionTests/**/*.cs",
                                BuildParameters.RootDirectoryPath + "/src/Cake.HgVersion/**/*.AssemblyInfo.cs"
                            });

Build.Run();