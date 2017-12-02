using VCSVersion.SemanticVersions;

namespace VCSVersion.VersionCalculation
{
    public interface IPreReleaseTagCalculator
    {
        PreReleaseTag CalculateTag(IVersionContext context, SemanticVersion semVersion, string branchNameOverride);
    }
}