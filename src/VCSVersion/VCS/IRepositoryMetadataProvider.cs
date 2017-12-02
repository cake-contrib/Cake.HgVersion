using System;
using System.Collections.Generic;
using System.Text;
using VCSVersion.SemanticVersions;

namespace VCSVersion.VCS
{
    /// <summary>
    /// Provides additional information about repository.
    /// </summary>
    public interface IRepositoryMetadataProvider
    {
        /// <summary>
        /// Find the commit where the given branch was branched from another branch.
        /// If there are multiple such commits and branches, tries to guess based on commit histories.
        /// </summary>
        ICommit FindCommitWasBranchedFrom(IBranchHead branch, params IBranchHead[] excludedBranches);

        /// <summary>
        /// Find the merge base of the two branches, i.e. the best common ancestor of the two branches' tips.
        /// </summary>
        ICommit FindMergeBase(IBranchHead branch, IBranchHead otherBranch);

        /// <summary>
        /// todo: add summary
        /// </summary>
        IEnumerable<IBranchHead> GetBranchesContainingCommit(ICommit commit, IList<IBranchHead> branches, bool onlyTrackedBranches);

        /// <summary>
        /// todo: add summary
        /// </summary>
        IEnumerable<SemanticVersion> GetVersionTagsOnBranch(IBranchHead branch, string tagPrefixRegex);

        /// <summary>
        /// todo: add summary
        /// </summary>
        IMergeMessage ParseMergeMessage(string message);
    }
}
