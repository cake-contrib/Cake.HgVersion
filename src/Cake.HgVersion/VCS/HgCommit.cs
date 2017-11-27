using System;
using System.Collections.Generic;
using HgVersion.VCS;
using Mercurial;

namespace Cake.HgVersion.VCS
{
    /// <inheritdoc />
    public sealed class HgCommit : ICommit
    {
        private readonly Changeset _changeset;

        /// <inheritdoc />
        public DateTime Timestamp => _changeset.Timestamp;

        /// <inheritdoc />
        public string AuthorName => _changeset.AuthorName;

        /// <inheritdoc />
        public string AuthorEmailAddress => _changeset.AuthorEmailAddress;

        /// <inheritdoc />
        public string CommitMessage => _changeset.CommitMessage;

        /// <inheritdoc />
        public string Branch => _changeset.Branch;
        
        /// <inheritdoc />
        public string Hash => _changeset.Hash;

        /// <inheritdoc />
        public IEnumerable<string> Tags => throw new NotImplementedException();

        /// <summary>
        /// Creates an instance of <see cref="HgCommit"/>
        /// </summary>
        /// <param name="changeset">Mercurial.Net <see cref="Changeset"/></param>
        public HgCommit(Changeset changeset)
        {
            _changeset = changeset;
        }
        
        /// <summary>
        /// Covert <see cref="Changeset"/> to <see cref="HgCommit"/>
        /// </summary>
        /// <param name="changeset">Changeset from Mercurial.Net</param>
        public static implicit operator HgCommit(Changeset changeset) =>
            new HgCommit(changeset);
    }
}
