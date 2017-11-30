using NUnit.Framework;
using VCSVersion.AssemblyVersioning;
using VCSVersion.SemanticVersions;

namespace VCSVersionTests.AssemblyVersioning
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class AssemblyFileVersionTests
    {
        [TestCase(AssemblyFileVersioningScheme.None, 1, 2, 3, 4, null)]
        [TestCase(AssemblyFileVersioningScheme.Major, 1, 2, 3, 4, "1.0.0.0")]
        [TestCase(AssemblyFileVersioningScheme.MajorMinor, 1, 2, 3, 4, "1.2.0.0")]
        [TestCase(AssemblyFileVersioningScheme.MajorMinorPatch, 1, 2, 3, 4, "1.2.3.0")]
        [TestCase(AssemblyFileVersioningScheme.MajorMinorPatchTag, 1, 2, 3, 4, "1.2.3.4")]
        public void GetAssemblyFileVersionTest(AssemblyFileVersioningScheme scheme, int major, int minor, int patch, int tag, string versionString)
        {
            var semVer = new SemanticVersion(major, minor, patch, preReleaseTag: new PreReleaseTag("Test", tag));
            var assemblyFileVersion = semVer.GetAssemblyFileVersion(scheme);

            Assert.That(assemblyFileVersion, Is.EqualTo(versionString));
        }
    }
}