using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using HgVersion;
using HgVersion.VCS;
using HgVersion.VersionAssemblyInfoResources;
using Mercurial;
using VCSVersion;
using VCSVersion.Output;
using VCSVersion.SemanticVersions;

// ReSharper disable UnusedMember.Global

namespace Cake.HgVersion
{
    /// <summary>
    /// Contains functionality for versioning using Mercurial history.
    /// <code>
    ///     #addin Cake.HgVersion
    /// </code>
    /// </summary>
    [CakeAliasCategory("HgVersion")]
    public static class Aliases
    {
        static Aliases()
        {
            Logger.SetLoggers(
                s => { /* empty */ },
                s => { /* empty */ },
                s => { /* empty */ },
                s => { /* empty */ });
        }

        /// <summary>
        /// Return information about current project version.
        /// </summary>
        /// <example>
        /// <code>
        ///     var version = GetVersion("./");
        /// </code>
        /// </example>
        /// <param name="context">Cake context</param>
        /// <param name="repositoryPath">Path to repository</param>
        /// <returns>Current project version</returns>
        [CakeMethodAlias]
        public static VersionVariables GetVersion(this ICakeContext context, DirectoryPath repositoryPath)
        {
            #region Arguments check (DupFinder Exclusion)
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));
            #endregion

            var path = repositoryPath.MakeAbsolute(context.Environment);

            using (context.AddLoggers())
            using (var repository = new Repository(path.FullPath))
            {
                var versionContext = new HgVersionContext((HgRepository)repository);

                var finder = new VersionFinder();
                var version = finder.FindVersion(versionContext);
                var variables = version.ToVersionVariables(versionContext);
            
                return variables;
            }
        }

        /// <summary>
        /// Set version tag to current commit.
        /// </summary>
        /// <example>
        /// <code>
        ///     if (SetVersionTag("./src", version))
        ///     {
        ///         Information("Semantic version tag set");
        ///     }
        /// </code>
        /// </example>
        /// <param name="context">Cake context</param>
        /// <param name="repositoryPath">Path to repository</param>
        /// <param name="variables">Version information variables</param>
        /// <returns>Returns true - if current commit has been tagged, otherwise - false</returns>
        [CakeMethodAlias]
        public static bool SetVersionTag(this ICakeContext context, DirectoryPath repositoryPath, VersionVariables variables)
        {
            #region Arguments check (DupFinder Exclusion)
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));
            #endregion

            var path = repositoryPath.MakeAbsolute(context.Environment);

            using (context.AddLoggers())
            using (var repository = new Repository(path.FullPath))
            {
                var version = SemanticVersion.Parse(variables.FullSemVer);
                var versionContext = new HgVersionContext((HgRepository)repository);

                if (versionContext.IsCurrentCommitTagged && version <= versionContext.CurrentCommitTaggedVersion)
                    return false;
                
                var commit = versionContext.CurrentCommit.Hash;
                var command = new TagCommand()
                    .WithName(variables.SemVer)
                    .WithRevision(commit);

                repository.Tag(command);
                return true;
            }
        }

        /// <summary>
        /// Update assembly info file or generate if threre is no such file.
        /// </summary>
        /// <example>
        /// <code>
        ///     UpdateAssemblyInfo(version, "./", "AssemblyInfo.cs");
        /// </code>
        /// </example>
        /// <param name="context">Cake context</param>
        /// <param name="variables">Version information variables</param>
        /// <param name="workingDirectory">Directory where assembly info file is located</param>
        /// <param name="assemblyInfoFile">Name of file with assemly information</param>
        [CakeMethodAlias]
        public static void UpdateAssemblyInfo(this ICakeContext context, VersionVariables variables, DirectoryPath workingDirectory, string assemblyInfoFile = "SolutionInfo.cs")
        {
            #region Arguments check (DupFinder Exclusion)
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (variables == null) throw new ArgumentNullException(nameof(variables));
            if (workingDirectory == null) throw new ArgumentNullException(nameof(workingDirectory));
            if (assemblyInfoFile == null) throw new ArgumentNullException(nameof(assemblyInfoFile));
            #endregion

            var fileSystem = new VCSVersion.Helpers.FileSystem();
            
            using (context.AddLoggers())
            using (var assemblyInfoFileUpdater = new AssemblyInfoFileUpdater(assemblyInfoFile, workingDirectory.FullPath, variables, fileSystem, true))
            {
                assemblyInfoFileUpdater.Update();
                assemblyInfoFileUpdater.CommitChanges();
            }
        }
    }
}