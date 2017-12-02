using System;
using System.Collections.Generic;
using System.Text;
using VCSVersion.SemanticVersions;

namespace VCSVersion.VCS
{
    /// <summary>
    /// todo: add summary
    /// </summary>
    public interface IMergeMessage
    {
        string TargetBranch { get; }
        string MergedBranch { get; }
        bool IsMergedPullRequest { get; }
        int? PullRequestNumber { get; }
        SemanticVersion Version { get; }
    }
}
