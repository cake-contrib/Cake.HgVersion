using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCSVersion.AssemblyVersioning;
using VCSVersion.Configuration;
using VCSVersion.VersionCalculation;
using VCSVersion.VersionCalculation.IncrementStrategies;
using VCSVersion.VersionFilters;

namespace VCSVersionTests.Configuration
{
    public class TestEffectiveConfiguration : EffectiveConfiguration
    {
        public TestEffectiveConfiguration(
            AssemblyVersioningScheme assemblyVersioningScheme = AssemblyVersioningScheme.MajorMinorPatch,
            AssemblyFileVersioningScheme assemblyFileVersioningScheme = AssemblyFileVersioningScheme.MajorMinorPatch,
            string assemblyInformationalFormat = null,
            VersioningMode versioningMode = VersioningMode.ContinuousDelivery,
            string gitTagPrefix = "v",
            string tag = "",
            string nextVersion = null,
            string branchPrefixToTrim = "",
            bool preventIncrementForMergedBranchVersion = false,
            string tagNumberPattern = null,
            string continuousDeploymentFallbackTag = "ci",
            bool trackMergeTarget = false,
            string majorMessage = null,
            string minorMessage = null,
            string patchMessage = null,
            string noBumpMessage = null,
            CommitMessageIncrementMode commitMessageMode = CommitMessageIncrementMode.Enabled,
            int buildMetaDataPadding = 4,
            int commitsSinceVersionSourcePadding = 4,
            IEnumerable<IVersionFilter> versionFilters = null,
            bool tracksReleaseBranches = false,
            bool isRelease = false,
            string commitDateFormat = "yyyy-MM-dd") :
            base(assemblyVersioningScheme, assemblyFileVersioningScheme, assemblyInformationalFormat, versioningMode, gitTagPrefix, tag, nextVersion, IncrementStrategyType.Patch,
                    branchPrefixToTrim, preventIncrementForMergedBranchVersion, tagNumberPattern, continuousDeploymentFallbackTag,
                    trackMergeTarget,
                    majorMessage, minorMessage, patchMessage, noBumpMessage,
                    commitMessageMode, buildMetaDataPadding, commitsSinceVersionSourcePadding,
                    versionFilters ?? Enumerable.Empty<IVersionFilter>(),
                    tracksReleaseBranches, isRelease, commitDateFormat)
        {
        }
    }
}
