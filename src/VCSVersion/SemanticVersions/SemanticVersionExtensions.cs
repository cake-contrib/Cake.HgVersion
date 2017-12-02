using VCSVersion.Configuration;
using VCSVersion.Output;

namespace VCSVersion.SemanticVersions
{
    /// <summary>
    /// <see cref="SemanticVersion"/> extension methods
    /// </summary>
    public static class SemanticVersionExtensions
    {
        /// <summary>
        /// Converts a <see cref="SemanticVersion"/> into <see cref="VersionVariables"/>
        /// </summary>
        /// <param name="semVersion">Semantic version</param>
        /// <param name="config">Effective configuration</param>
        /// <returns></returns>
        public static VersionVariables ToVersionVariables(this SemanticVersion semVersion, EffectiveConfiguration config)
        {
            var builder = new VersionVariablesBuilder(semVersion, config);
            return builder.Build();
        }
    }
}