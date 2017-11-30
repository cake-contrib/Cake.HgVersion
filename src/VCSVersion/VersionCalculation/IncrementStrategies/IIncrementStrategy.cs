using VCSVersion.SemanticVersions;

namespace VCSVersion.VersionCalculation
{
    public interface IIncrementStrategy
    {
        SemanticVersion IncrementVersion(SemanticVersion semver);
    }
}