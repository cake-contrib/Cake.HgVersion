using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion.VersionFilters
{
    public interface IVersionFilter
    {
        bool Exclude(BaseVersion baseVersion, out string reason);
    }
}
