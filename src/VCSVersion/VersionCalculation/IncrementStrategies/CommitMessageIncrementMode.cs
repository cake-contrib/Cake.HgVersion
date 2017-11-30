using System;
using System.Collections.Generic;
using System.Text;

namespace VCSVersion.VersionCalculation.IncrementStrategies
{
    public enum CommitMessageIncrementMode
    {
        Enabled,
        Disabled,
        MergeMessageOnly
    }
}
