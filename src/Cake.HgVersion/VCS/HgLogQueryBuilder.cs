using System;
using HgVersion.VCS;
using Mercurial;

namespace Cake.HgVersion.VCS
{
    /// <inheritdoc />
    public sealed class HgLogQueryBuilder : ILogQueryBuilder
    {
        /// <inheritdoc />
        public ILogQuery Single(string hash)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentNullException(nameof(hash));

            return (HgLogQuery) RevSpec.Single(hash);
        }

        /// <inheritdoc />
        public ILogQuery AncestorsOf(ILogQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            
            if (!(query is HgLogQuery hgQuery))
                throw new InvalidOperationException($"{query.GetType()} is not supported.");

            return (HgLogQuery) RevSpec.AncestorsOf(hgQuery.Revision);
        }
    }
}