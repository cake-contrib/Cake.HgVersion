using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation
{
    public interface IMetadataCalculator
    {
        BuildMetadata CalculateMetadata(IVersionContext context, ICommit baseVersionSource);
    }
}