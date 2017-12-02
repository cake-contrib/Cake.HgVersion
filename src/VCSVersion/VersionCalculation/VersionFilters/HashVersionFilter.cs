using System;
using System.Collections.Generic;
using System.Linq;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersion.VersionFilters
{
    public sealed class HashVersionFilter : IVersionFilter
    {
        private readonly IEnumerable<string> _hashes;

        public HashVersionFilter(IEnumerable<string> hashes)
        {
            if (hashes == null)
                throw new ArgumentNullException(nameof(hashes));

            _hashes = hashes;
        }

        public bool Exclude(BaseVersion baseVersion, out string reason)
        {
            if (baseVersion == null)
                throw new ArgumentNullException(nameof(baseVersion));

            reason = null;

            if (baseVersion.Source != null &&
                _hashes.Any(hash => baseVersion.Source.Hash.StartsWith(hash, StringComparison.OrdinalIgnoreCase)))
            {
                reason = $"Hash {baseVersion.Source.Hash} was ignored due to commit having been excluded by configuration";
                return true;
            }

            return false;
        }
    }
}
