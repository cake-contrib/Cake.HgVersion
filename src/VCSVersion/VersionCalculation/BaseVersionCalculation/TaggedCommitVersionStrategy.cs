using System;
using System.Collections.Generic;
using System.Linq;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;

namespace VCSVersion.VersionCalculation.BaseVersionCalculation
{
    /// <summary>
    /// Version is extracted from all tags on the branch which are valid, and not newer than the current commit.
    /// <see cref="BaseVersion.Source"/> is the tag's commit.
    /// Increments if the tag is not the current commit.
    /// </summary>
    public sealed class TaggedCommitVersionStrategy : IBaseVersionStrategy
    {
        public IEnumerable<BaseVersion> GetVersions(IVersionContext context)
        {
            return GetTaggedVersions(context, context.CurrentBranch, context.CurrentCommit.When);
        }

        public IEnumerable<BaseVersion> GetTaggedVersions(IVersionContext context, IBranchHead currentBranch, DateTimeOffset? olderThan)
        {
            var allTags = context.Repository.Tags()
                .Where(tag => !olderThan.HasValue || tag.Commit.When <= olderThan.Value)
                .ToList();

            var tagsOnBranch = currentBranch
                .Commits(context)
                .SelectMany(commit => allTags.Where(t => IsValidTag(t, commit)))
                .Select(t =>
                {
                    if (SemanticVersion.TryParse(t.Name, context.Configuration.TagPrefix, out SemanticVersion version))
                    {
                        var commit = t.Commit;
                        if (commit != null)
                            return new VersionTaggedCommit(commit, version, t.Name);
                    }

                    return null;
                })
                .Where(a => a != null)
                .ToList();

            return tagsOnBranch.Select(t => CreateBaseVersion(context, t));
        }

        private static BaseVersion CreateBaseVersion(IVersionContext context, VersionTaggedCommit version)
        {
            var shouldUpdateVersion = version.Commit.Hash != context.CurrentCommit.Hash;
            return new BaseVersion(FormatType(version), version.SemVer, version.Commit, shouldUpdateVersion);
        }

        private static string FormatType(VersionTaggedCommit version)
        {
            return string.Format("Git tag '{0}'", version.Tag);
        }

        private static bool IsValidTag(ITag tag, ICommit commit)
        {
            return tag.Commit == commit;
        }

        private sealed class VersionTaggedCommit
        {
            public string Tag;
            public ICommit Commit;
            public SemanticVersion SemVer;

            public VersionTaggedCommit(ICommit commit, SemanticVersion semVer, string tag)
            {
                Tag = tag;
                Commit = commit;
                SemVer = semVer;
            }
        }
    }
}
