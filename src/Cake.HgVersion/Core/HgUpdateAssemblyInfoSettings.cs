// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable InheritdocConsiderUsage

namespace Cake.HgVersion.Core
{
    /// <summary>
    /// Contains settings used by <see cref="HgVersionExtensions.UpdateAssemblyInfo"/>
    /// </summary>
    public class HgUpdateAssemblyInfoSettings : HgIncrementVersionSettings
    {
        /// <summary>
        /// Relative AssemblyInfo.cs file path
        /// </summary>
        public string AssembyInfoPath { get; set; }

        /// <inheritdoc />
        public HgUpdateAssemblyInfoSettings()
        {
            AssembyInfoPath = "Properties/AssemblyInfo.cs";
        }
    }
}
