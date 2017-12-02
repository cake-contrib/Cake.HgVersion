using System;
using System.Collections.Generic;
using System.Linq;

namespace VCSVersion.VCS
{
    /// <summary>
    /// Extension methods for VCS entities
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Retrieve the parents of the current commit.
        /// </summary>
        /// <param name="commit">The current commit</param>
        /// <param name="context">The current semantic versioning proccess context</param>
        public static IEnumerable<ICommit> Parents(this ICommit commit, IVersionContext context) =>
            context.Repository.Parents(commit);
        
        /// <summary>
        /// todo: add summary
        /// </summary>
        public static IEnumerable<ICommit> CommitsPriorToThan(this IBranchHead branch, IVersionContext context, DateTimeOffset olderThan) =>
            branch.Commits(context).SkipWhile(c => c.When > olderThan);

        /// <summary>
        /// todo: add summary
        /// </summary>
        public static IEnumerable<ICommit> Commits(this IBranchHead branch, IVersionContext context) =>
            context.Repository.Log(select => select.ByBranch(branch.Name));
    }
}
