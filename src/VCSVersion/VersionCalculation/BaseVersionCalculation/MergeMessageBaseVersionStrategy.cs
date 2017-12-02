using System.Collections.Generic;
using System.Linq;
using VCSVersion.Configuration;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation.BaseVersionCalculation
{
    /// <summary>
    /// Version is extracted from older commits's merge messages.
    /// <see cref="BaseVersion.Source"/> is the commit where the message was found.
    /// Increments if <see cref="BranchConfig.PreventIncrementForMergedBranchVersion"/> is false.
    /// </summary>
    public sealed class MergeMessageBaseVersionStrategy : IBaseVersionStrategy
    {
        public IEnumerable<BaseVersion> GetVersions(IVersionContext context)
        {
            var commitsPriorToThan = context.CurrentBranch
                .CommitsPriorToThan(context, context.CurrentCommit.When);

            var baseVersions = commitsPriorToThan
                .SelectMany(c =>
                {
                    if (TryParse(c, context, out var semanticVersion))
                    {
                        var shouldIncrement = !context.Configuration.PreventIncrementForMergedBranchVersion;
                        var baseVersion = new BaseVersion(FormatType(c), semanticVersion, c, shouldIncrement);

                        return new[] { baseVersion };
                    }

                    return Enumerable.Empty<BaseVersion>();
                }).ToList();

            return baseVersions;
        }

        private static string FormatType(ICommit commit)
        {
            return $"Merge message '{commit.CommitMessage?.Trim()}'";
        }

        private static bool TryParse(ICommit mergeCommit, IVersionContext context, out SemanticVersion semanticVersion)
        {
            semanticVersion = Inner(mergeCommit, context);
            return semanticVersion != null;
        }

        private static SemanticVersion Inner(ICommit mergeCommit, IVersionContext context)
        {
            if (mergeCommit.Parents(context).Count() < 2)
                return null;

            var mergeMessage = context
                .RepositoryMetadataProvider
                .ParseMergeMessage(mergeCommit.CommitMessage);

            return mergeMessage.Version;
        }
    }
}
