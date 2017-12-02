using System;
using System.Collections.Generic;
using System.Text;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;

namespace VCSVersion
{
    public class VersionFinder
    {
        public SemanticVersion FindVersion(IVersionContext context)
        {
            var branchName = context.CurrentBranch.Name;
            var commit = context.CurrentCommit == null ? "-" : context.CurrentCommit.Hash;
            Logger.WriteInfo($"Running against branch: {branchName} ({commit})");

            if (context.IsCurrentCommitTagged)
            {
                Logger.WriteInfo(
                    $"Current commit is tagged with version {context.CurrentCommitTaggedVersion}, " 
                    + "version calcuation is for metadata only.");
            }

            return new NextVersionCalculator().CalculateVersion(context);
        }
    }
}
