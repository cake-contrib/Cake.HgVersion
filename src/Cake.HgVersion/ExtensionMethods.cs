using System;
using Cake.Core;
using Cake.Core.Diagnostics;
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

        public static IDisposable AddLoggers(this ICakeContext context)
        {
            return Logger.AddLoggersTemporarily(
                s => context.Log.Debug(s),
                s => context.Log.Information(s),
                s => context.Log.Warning(s),
                s => context.Log.Error(s));
        }
    }
}