using VCSVersion.SemanticVersions;

namespace VCSVersion.VersionCalculation
{
    public interface IVersionCalculator
    {
        SemanticVersion CalculateVersion(IVersionContext context);
    }
}
