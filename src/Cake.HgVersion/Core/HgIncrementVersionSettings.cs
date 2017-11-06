using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Cake.HgVersion.Core
{
    /// <summary>
    /// Contains settings used by <see cref="HgVersionExtensions.TryIncrementVersion"/>
    /// </summary>
    public class HgIncrementVersionSettings : HgVersionSettings
    {
        /// <summary>
        /// Relative project path
        /// </summary>
        public string ProjectPath { get; set; }

        /// <summary>
        /// Project version increment strategy
        /// </summary>
        public Func<Version, Version> Increment { get; set; }
        
        /// <inheritdoc />
        public HgIncrementVersionSettings()
        {
            ProjectPath = string.Empty;
            Increment = DefaultIncrement;
        }

        private static Version DefaultIncrement(Version version)
        {
            var major = version.Major;
            var minor = version.Minor;
            var build = version.Build;
            var revision = version.Revision;

            if (revision != -1)
                return new Version(major, minor, build, revision + 1);

            if (build != -1)
                return new Version(major, minor, build + 1);

            return new Version(major, minor + 1);
        }
    }
}
