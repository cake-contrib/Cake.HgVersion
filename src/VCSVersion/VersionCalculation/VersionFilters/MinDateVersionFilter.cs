using System;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion.VersionFilters
{
    public sealed class MinDateVersionFilter : IVersionFilter
    {
        private readonly DateTimeOffset _minimum;

        public MinDateVersionFilter(DateTimeOffset minimum)
        {
            _minimum = minimum;
        }

        public bool Exclude(BaseVersion baseVersion, out string reason)
        {
            if (baseVersion == null)
                throw new ArgumentNullException(nameof(baseVersion));

            reason = null;

            if (baseVersion.Source != null &&
                baseVersion.Source.When < _minimum)
            {
                reason = "Source was ignored due to commit date being outside of configured range";
                return true;
            }

            return false;
        }
    }
}
