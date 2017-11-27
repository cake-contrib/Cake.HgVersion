using HgVersion;
using HgVersion.AssemblyVersioning;
using HgVersion.SemanticVersions;
using NUnit.Framework;

namespace HgVersionTests.AssemblyVersioning
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class AssemblyVersionTests
    {
        [TestCase(AssemblyVersioningScheme.None, 1, 2, 3, 4, null)]
        [TestCase(AssemblyVersioningScheme.Major, 1, 2, 3, 4, "1.0.0.0")]
        [TestCase(AssemblyVersioningScheme.MajorMinor, 1, 2, 3, 4, "1.2.0.0")]
        [TestCase(AssemblyVersioningScheme.MajorMinorPatch, 1, 2, 3, 4, "1.2.3.0")]
        [TestCase(AssemblyVersioningScheme.MajorMinorPatchTag, 1, 2, 3, 4, "1.2.3.4")]
        public void GetAssemblyVersionTest(AssemblyVersioningScheme scheme, int major, int minor, int patch, int tag, string versionString)
        {
            var semVer = new SemanticVersion(major, minor, patch, preReleaseTag: new PreReleaseTag("Test", tag));
            var assemblyVersion = semVer.GetAssemblyVersion(scheme);

            Assert.That(assemblyVersion, Is.EqualTo(versionString));
        }
    }
}