namespace VCSVersion.VersionCalculation.IncrementStrategies
{
    public enum IncrementStrategyType
    {
        None,
        Major,
        Minor,
        Patch,
        /// <summary>
        /// Uses the <see cref="BranchConfig.Increment"/>, <see cref="BranchConfig.PreventIncrementOfMergedBranchVersion"/> and <see cref="BranchConfig.TracksReleaseBranches"/>
        /// of the "parent" branch (i.e. the branch where the current branch was branched from).
        /// </summary>
        Inherit
    }
}
