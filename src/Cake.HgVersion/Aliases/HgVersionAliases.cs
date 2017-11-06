using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.HgVersion.Core;
using Mercurial;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Cake.HgVersion.Aliases
{
    public static class HgVersionAliases
    {
        /// <summary>
        /// Get the mercurial version info for the project. 
        /// </summary>
        /// <param name="context">Cake context</param>
        /// <param name="repositoryPath">Path to repository</param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Version")]
        public static HgVersionInfo HgVersion(this ICakeContext context, DirectoryPath repositoryPath)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));

            var path = repositoryPath.MakeAbsolute(context.Environment);
            return new Repository(path.FullPath).GetVersionInfo();
        }

        /// <summary>
        /// Get the mercurial version info for the project. 
        /// </summary>
        /// <param name="context">Cake context</param>
        /// <param name="repositoryPath">Path to repository</param>
        /// <param name="settings">Hg version settings</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Version")]
        public static HgVersionInfo HgVersion(this ICakeContext context, DirectoryPath repositoryPath, HgVersionSettings settings)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (repositoryPath == null) throw new ArgumentNullException(nameof(repositoryPath));

            var path = repositoryPath.MakeAbsolute(context.Environment);
            return new Repository(path.FullPath).GetVersionInfo(settings);
        }
    }
}