using VCSVersion;
using VCSVersion.Output;
using VCSVersion.SemanticVersions;

namespace Cake.HgVersion
{
    internal static class ExtensionMethods
    {
        public static VersionVariables ToVersionVariables(this SemanticVersion version, IVersionContext context)
        {
            return version.ToVersionVariables(context.Configuration, context.IsCurrentCommitTagged);
        }
    }
}