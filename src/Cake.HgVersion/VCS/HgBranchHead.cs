using System;
using HgVersion.VCS;

namespace Cake.HgVersion.VCS
{
    /// <inheritdoc cref="HgNamedCommit" />
    public sealed class HgBranchHead : HgNamedCommit, IBranchHead
    {
        /// <summary>
        /// Creates an instance of <see cref="HgBranchHead"/>.
        /// </summary>
        /// <param name="name">Commit name</param>
        /// <param name="commit">Commit itself</param>
        public HgBranchHead(string name, ICommit commit) : base(name, commit)
        { }
        
        /// <summary>
        /// Creates an instance of <see cref="HgBranchHead"/>
        /// </summary>
        /// <param name="name">Commit name</param>
        /// <param name="commitLookup">Commit lookup function</param>
        public HgBranchHead(string name, Func<ICommit> commitLookup) : base(name, commitLookup)
        { }
    }
}