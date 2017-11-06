using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Mercurial;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Cake.HgVersion.Core
{
    /// <summary>
    /// Extension methods for projects versioning with semantic version standard 
    /// </summary>
    public static class HgVersionExtensions
    {
        private static readonly Regex AssemblyVersionPattern = new Regex(
            @"^(\s*\[\s*assembly\s*:\s*((System\s*\.)?\s*Reflection\s*\.)?\s*AssemblyVersion\()(.*)(\)\])"
            , RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static readonly Regex AssemblyFileVersionPattern = new Regex(
            @"^(\s*\[\s*assembly\s*:\s*((System\s*\.)?\s*Reflection\s*\.)?\s*AssemblyFileVersion\()(.*)(\)\])"
            , RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static readonly Regex TagVersionPattern = new Regex(
            @"(?<proj>[^\s\d]*)?\s*(?<ver>(\d+\.)*(\d+))"
            , RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Add version info tag for changeset.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="info">Project version info</param>
        public static void Tag(this Repository repository, HgVersionInfo info)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (info == null)
                throw new ArgumentNullException(nameof(info));

            repository.Tag(new TagCommand()
                .WithName($"{info.Project} {info.Version}")
                .WithRevision(info.Changeset.Revision));
        }

        /// <summary>
        /// Get project version info.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="settings">Hg version settings</param>
        /// <returns>Project version info</returns>
        public static HgVersionInfo GetVersionInfo(this Repository repository, HgVersionSettings settings = null)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            // todo: add branch filtering
            settings = settings ?? new HgVersionSettings();
            var resultTag = repository.Tags()
                .LastOrDefault(tag => IsMatch(tag, settings));

            if (resultTag == null)
                return new HgVersionInfo(settings.ProjectName);

            return repository.GetVersionInfo(resultTag);
        }

        /// <summary>
        /// Get project version info by tag.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="tag">Mercurial tag</param>
        /// <returns>Project version info</returns>
        public static HgVersionInfo GetVersionInfo(this Repository repository, Tag tag)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            var match = TagVersionPattern.Match(tag.Name);
            var project = match.Groups["proj"].Value;
            var version = new Version(match.Groups["ver"].Value);
            var changeset = repository.Changeset(tag.RevisionNumber);

            return new HgVersionInfo(project, version, changeset);
        }

        /// <summary>
        /// Commit project version info.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="info">Project version info</param>
        public static void CommitVersionInfo(this Repository repository, HgVersionInfo info)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            if (info == null)
                throw new ArgumentNullException(nameof(info));

            // get actual changeset information
            var changeset = repository.Changeset(info.Changeset.Revision);
            var expected = $"{info.Project} {info.Version}";

            if (changeset.Tags.Any(tag => StringComparer.OrdinalIgnoreCase.Equals(tag, expected)))
                return;

            repository.Tag(info);
        }

        /// <summary>
        /// Update AssemblyInfo.cs file with actual project version.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="settings">Hg update assembly version info settings</param>
        public static void UpdateAssemblyInfo(this Repository repository, HgUpdateAssemblyInfoSettings settings = null)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            
            settings = settings ?? new HgUpdateAssemblyInfoSettings();
            repository.TryIncrementVersion(out var info, settings);

            var path = Path.Combine(repository.Path, settings.AssembyInfoPath);
            var content = File.ReadAllText(path);
            var version = info.Version;

            content = AssemblyVersionPattern.Replace(content, $"$1\"{version}\"$5");
            content = AssemblyFileVersionPattern.Replace(content, $"$1\"{version}\"$5");

            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Try to get next project version by its change history.
        /// </summary>
        /// <param name="repository">Mercurial repository</param>
        /// <param name="info">Next project version, if there are any project changes, otherwise current project version</param>
        /// <param name="settings">Hg increment version settings</param>
        /// <returns>true - if there are any project changes, otherwise false</returns>
        public static bool TryIncrementVersion(this Repository repository, out HgVersionInfo info, HgIncrementVersionSettings settings = null)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            
            settings = settings ?? new HgIncrementVersionSettings();
            info = repository.GetVersionInfo(settings);

            var tip = repository.Changeset(RevSpec.ByBranch(settings.Branch));
            var revisions = GetDiffRevisions(info, tip);

            var diff = repository.Diff(new DiffCommand()
                .WithNames(settings.ProjectPath)
                .WithRevisions(revisions));

            if (string.IsNullOrEmpty(diff))
                return false;

            var nextVersion = settings.Increment(info.Version);
            info = new HgVersionInfo(info.Project, nextVersion, tip);

            return true;
        }

        private static RevSpec GetDiffRevisions(HgVersionInfo info, Changeset tip)
        {
            var from = info?.Changeset?.Revision ?? RevSpec.Null;
            var to = tip?.Revision ?? RevSpec.Null;

            return RevSpec.Range(from, to);
        }

        private static bool IsMatch(Tag tag, HgVersionSettings settings)
        {
            var match = TagVersionPattern.Match(tag.Name);
            return match.Success && IsProjectMatch(match, settings);
        }

        private static bool IsProjectMatch(Match match, HgVersionSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ProjectName))
                return true;

            return StringComparer.OrdinalIgnoreCase.Equals(match.Groups["proj"].Value, settings.ProjectName);
        }
    }
}