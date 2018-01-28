#addin "Cake.Hg"
#addin "Cake.HgVersion"

using System.Linq;
using VCSVersion.Output;

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target          = Argument<string>("target", "Default");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var testRepo        = MakeAbsolute(Directory("./tests/repo"));
var versionVars     = (VersionVariables)null;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
    Information("Testing \"Cake.HgVersion Addin\"");
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    // Information("Finished running tasks.");

    try
    {
        Information("Trying to clean up test repo {0}", testRepo);
        if (DirectoryExists(testRepo))
        {
            ForceDeleteDirectory(testRepo.FullPath);
        }
        if (DirectoryExists(testRepo))
        {
            Warning("Failed to clean {0}", testRepo);
        }
        else
        {
            Information("Successfully cleaned test repo {0}", testRepo);
        }
    }
    catch(Exception ex)
    {
        Error("Failed to clean up test reop {0}\r\n{1}", testRepo, ex);
    }
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    // Clean up if repo exists
    if (DirectoryExists(testRepo))
    {
        ForceDeleteDirectory(testRepo.FullPath);
    }
});

Task("Create-Repository")
    .IsDependentOn("Clean")
    .Does(() =>
{
    Information("Creating repository...");
    CreateDirectory(testRepo);
    HgInit(testRepo);
    
    if (!DirectoryExists(testRepo + "/.hg"))
    {
        throw new DirectoryNotFoundException($"Failed to create repository {testRepo}");
    }

    Information("Repository created {0}.", testRepo);
    Information("Creating initial commit...");

    CreateFile(Context, testRepo + "/init.txt");
    HgCommit(testRepo, "Inital commit");
    var initCommit = HgTip(testRepo).Hash;

    if (string.IsNullOrEmpty(initCommit))
    {
        throw new Exception($"Failed to commit initial state {testRepo}");
    }

    Information("Commit created {0}", initCommit);
});

Task("GetVersion")
    .IsDependentOn("Create-Repository")
    .Does(() =>
{
    Information("Getting version...");
    versionVars = GetVersion(testRepo);
    
    if (versionVars == null)
    {
        throw new Exception($"Failed to get semantic version {testRepo}");
    }

    Information("Semantic version\n{0}.", versionVars);
});

Task("UpdateAssemblyInfo")
    .IsDependentOn("GetVersion")
    .Does(() =>
{
    Information("Updating AssemblyInfo.cs file...");
    
    UpdateAssemblyInfo(versionVars, testRepo, "AssemblyInfo.cs");

    if (System.IO.File.Exists(testRepo + "AssemblyInfo.cs"))
    {
        throw new Exception("Failed to update AssemblyInfo.cs file");
    }

    Information("AssemblyInfo.cs file updated");
});

Task("SetVersionTag")
    .IsDependentOn("GetVersion")
    .Does(() =>
{
    Information("Setting semantic version tag...");
    
    if (!SetVersionTag(testRepo, versionVars))
    {
        throw new Exception("Failed to set semantic version tag");
    }

    var tag = HgTags(testRepo)
        .Where(t => t.Name != "tip")
        .SingleOrDefault();

    if (tag == null)
    {
        throw new Exception("Failed to set semantic version tag");
    }

    Information("Semantic version tag {0}", tag.Name);
});

Task("Default")
    .IsDependentOn("GetVersion")
    .IsDependentOn("UpdateAssemblyInfo")
    .IsDependentOn("SetVersionTag");

RunTarget(target);

public static void ForceDeleteDirectory(string path)
{
    var directory = new System.IO.DirectoryInfo(path) { Attributes = FileAttributes.Normal };

    foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
    {
        info.Attributes = FileAttributes.Normal;
    }

    directory.Delete(true);
}

public static void CreateFile(ICakeContext context, FilePath filePath)
{
    var file = context.FileSystem.GetFile(filePath);
    var guid = new Guid();

    using(var stream = file.OpenWrite())
    using(var writer = new System.IO.StreamWriter(stream, Encoding.ASCII))
    {
        writer.WriteLine(guid);
    }
}