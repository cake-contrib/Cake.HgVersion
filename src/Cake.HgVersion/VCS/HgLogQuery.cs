using HgVersion.VCS;
using Mercurial;

// ReSharper disable MemberCanBePrivate.Global

namespace Cake.HgVersion.VCS
{
    /// <inheritdoc />
    public sealed class HgLogQuery : ILogQuery
    {
        /// <summary>
        /// Specifies a set of revisions.
        /// </summary>
        public RevSpec Revision { get; }

        /// <summary>
        /// Create an instance of <see cref="HgLogQuery"/>
        /// </summary>
        /// <param name="revision">Specifies a set of revisions</param>
        public HgLogQuery(RevSpec revision)
        {
            Revision = revision;
        }

        /// <inheritdoc />
        public ILogQuery Limit(int amount)
        {
            return (HgLogQuery)Revision.Limit(amount);
        }

        /// <summary>
        /// Converts a <see cref="RevSpec"/> into a <see cref="HgLogQuery"/> 
        /// </summary>
        /// <param name="revision">Specifies a set of revisions</param>
        /// <returns></returns>
        public static implicit operator HgLogQuery(RevSpec revision) =>
            new HgLogQuery(revision);
    }
}