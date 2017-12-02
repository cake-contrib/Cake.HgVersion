using System.Collections.Generic;
using VCSVersion.AssemblyVersioning;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.IncrementStrategies;
using VCSVersion.VersionFilters;

namespace VCSVersion.Configuration
{
    /// <summary>
    /// Configuration can be applied to different things, effective configuration 
    /// is the result after applying the appropriate configuration
    /// </summary>
    public class EffectiveConfiguration
    {
        public EffectiveConfiguration(
            AssemblyVersioningScheme assemblyVersioningScheme,
            AssemblyFileVersioningScheme assemblyFileVersioningScheme,
            string assemblyInformationalFormat,
            VersioningMode versioningMode, string tagPrefix,
            string tag, string nextVersion, IncrementStrategyType increment,
            string branchPrefixToTrim,
            bool preventIncrementForMergedBranchVersion,
            string tagNumberPattern,
            string continuousDeploymentFallbackTag,
            bool trackMergeTarget,
            string majorVersionBumpMessage,
            string minorVersionBumpMessage,
            string patchVersionBumpMessage,
            string noBumpMessage,
            CommitMessageIncrementMode commitMessageIncrementing,
            int buildMetaDataPadding,
            int commitsSinceVersionSourcePadding,
            IEnumerable<IVersionFilter> versionFilters,
            bool tracksReleaseBranches,
            bool isCurrentBranchRelease,
            string commitDateFormat)
        {
            AssemblyVersioningScheme = assemblyVersioningScheme;
            AssemblyFileVersioningScheme = assemblyFileVersioningScheme;
            AssemblyInformationalFormat = assemblyInformationalFormat;
            VersioningMode = versioningMode;
            TagPrefix = tagPrefix;
            Tag = tag;
            NextVersion = nextVersion;
            Increment = increment;
            BranchPrefixToTrim = branchPrefixToTrim;
            PreventIncrementForMergedBranchVersion = preventIncrementForMergedBranchVersion;
            TagNumberPattern = tagNumberPattern;
            ContinuousDeploymentFallbackTag = continuousDeploymentFallbackTag;
            TrackMergeTarget = trackMergeTarget;
            MajorVersionBumpMessage = majorVersionBumpMessage;
            MinorVersionBumpMessage = minorVersionBumpMessage;
            PatchVersionBumpMessage = patchVersionBumpMessage;
            NoBumpMessage = noBumpMessage;
            CommitMessageIncrementing = commitMessageIncrementing;
            BuildMetaDataPadding = buildMetaDataPadding;
            CommitsSinceVersionSourcePadding = commitsSinceVersionSourcePadding;
            VersionFilters = versionFilters;
            TracksReleaseBranches = tracksReleaseBranches;
            IsCurrentBranchRelease = isCurrentBranchRelease;
            CommitDateFormat = commitDateFormat;
        }

        public bool TracksReleaseBranches { get; }
        public bool IsCurrentBranchRelease { get; }

        public VersioningMode VersioningMode { get; }

        public AssemblyVersioningScheme AssemblyVersioningScheme { get; }
        public AssemblyFileVersioningScheme AssemblyFileVersioningScheme { get; }
        public string AssemblyInformationalFormat { get; }

        /// <summary>
        /// Tag prefix
        /// </summary>
        public string TagPrefix { get; }

        /// <summary>
        /// Tag to use when calculating SemVer
        /// </summary>
        public string Tag { get; }

        public string NextVersion { get; }

        public IncrementStrategyType Increment { get; }

        public string BranchPrefixToTrim { get; }

        public bool PreventIncrementForMergedBranchVersion { get; }

        public string TagNumberPattern { get; }

        public string ContinuousDeploymentFallbackTag { get; }

        public bool TrackMergeTarget { get; }

        public string MajorVersionBumpMessage { get; }

        public string MinorVersionBumpMessage { get; }

        public string PatchVersionBumpMessage { get; }

        public string NoBumpMessage { get; }
        public int BuildMetaDataPadding { get; }

        public int CommitsSinceVersionSourcePadding { get; }

        public CommitMessageIncrementMode CommitMessageIncrementing { get; }

        public IEnumerable<IVersionFilter> VersionFilters { get; }

        public string CommitDateFormat { get; }
    }
}
