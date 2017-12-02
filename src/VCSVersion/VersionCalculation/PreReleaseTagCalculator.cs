using System.Linq;
using System.Text.RegularExpressions;
using VCSVersion.Configuration;
using VCSVersion.SemanticVersions;

namespace VCSVersion.VersionCalculation
{
    public sealed class PreReleaseTagCalculator : IPreReleaseTagCalculator
    {
        public PreReleaseTag CalculateTag(IVersionContext context, SemanticVersion semVersion, string branchNameOverride)
        {
            var comparer = new SemanticVersionComarer(SemanticVersionComparation.MajorMinorPatch);
            var tagToUse = GetBranchSpecificTag(context.Configuration, context.CurrentBranch.Name, branchNameOverride);

            var lastTag = context.RepositoryMetadataProvider
                .GetVersionTagsOnBranch(context.CurrentBranch, context.Configuration.TagPrefix)
                .FirstOrDefault(v => v.PreReleaseTag.Name == tagToUse);

            var baseVersionNotChanged = lastTag != null &&
                !lastTag.PreReleaseTag.IsNull() && 
                comparer.Equals(lastTag, semVersion);

            if (baseVersionNotChanged)
                return new PreReleaseTag(tagToUse, lastTag.PreReleaseTag.Number + 1);

            return new PreReleaseTag(tagToUse, 1);
        }

        public static string GetBranchSpecificTag(EffectiveConfiguration configuration, string branchFriendlyName, string branchNameOverride)
        {
            var tagToUse = configuration.Tag;
            if (tagToUse == "useBranchName")
            {
                tagToUse = "{BranchName}";
            }
            if (tagToUse.Contains("{BranchName}"))
            {
                var branchName = branchNameOverride ?? branchFriendlyName;
                if (!string.IsNullOrWhiteSpace(configuration.BranchPrefixToTrim))
                {
                    branchName = Regex.Replace(branchName, configuration.BranchPrefixToTrim, string.Empty, RegexOptions.IgnoreCase);
                }
                branchName = Regex.Replace(branchName, "[^a-zA-Z0-9-]", "-");

                tagToUse = tagToUse.Replace("{BranchName}", branchName);
            }
            return tagToUse;
        }
    }
}