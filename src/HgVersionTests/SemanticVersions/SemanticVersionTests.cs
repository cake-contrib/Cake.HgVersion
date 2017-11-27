using HgVersion.SemanticVersions;
using NUnit.Framework;

namespace HgVersionTests.SemanticVersions
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class SemanticVersionTests
    {
        [TestCase("1.2.3", 1, 2, 3, null, null, null, null, null, null, null, null)]
        [TestCase("1.2", 1, 2, 0, null, null, null, null, null, null, "1.2.0", null)]
        [TestCase("1.2.3-beta", 1, 2, 3, "beta", null, null, null, null, null, null, null)]
        [TestCase("1.2.3-beta3", 1, 2, 3, "beta", 3, null, null, null, null, "1.2.3-beta.3", null)]
        [TestCase("1.2.3-beta.3", 1, 2, 3, "beta", 3, null, null, null, null, "1.2.3-beta.3", null)]
        [TestCase("1.2.3-beta-3", 1, 2, 3, "beta-3", null, null, null, null, null, "1.2.3-beta-3", null)]
        [TestCase("1.2.3-alpha", 1, 2, 3, "alpha", null, null, null, null, null, null, null)]
        [TestCase("1.2-alpha4", 1, 2, 0, "alpha", 4, null, null, null, null, "1.2.0-alpha.4", null)]
        [TestCase("1.2.3-rc", 1, 2, 3, "rc", null, null, null, null, null, null, null)]
        [TestCase("1.2.3-rc3", 1, 2, 3, "rc", 3, null, null, null, null, "1.2.3-rc.3", null)]
        [TestCase("1.2.3-RC3", 1, 2, 3, "RC", 3, null, null, null, null, "1.2.3-RC.3", null)]
        [TestCase("1.2.3-rc3.1", 1, 2, 3, "rc3", 1, null, null, null, null, "1.2.3-rc3.1", null)]
        [TestCase("01.02.03-rc03", 1, 2, 3, "rc", 3, null, null, null, null, "1.2.3-rc.3", null)]
        [TestCase("1.2.3-beta3f", 1, 2, 3, "beta3f", null, null, null, null, null, null, null)]
        [TestCase("1.2.3-notAStability1", 1, 2, 3, "notAStability", 1, null, null, null, null, "1.2.3-notAStability.1", null)]
        [TestCase("1.2.3.4", 1, 2, 3, null, null, 4, null, null, null, "1.2.3+4", null)]
        [TestCase("1.2.3+4", 1, 2, 3, null, null, 4, null, null, null, null, null)]
        [TestCase("1.2.3+4.Branch.Foo", 1, 2, 3, null, null, 4, "Foo", null, null, null, null)]
        [TestCase("1.2.3+randomMetaData", 1, 2, 3, null, null, null, null, null, "randomMetaData", null, null)]
        [TestCase("1.2.3-beta.1+4.Sha.12234.Othershiz", 1, 2, 3, "beta", 1, 4, null, "12234", "Othershiz", null, null)]
        [TestCase("1.2.3", 1, 2, 3, null, null, null, null, null, null, null, "[vV]")]
        [TestCase("v1.2.3", 1, 2, 3, null, null, null, null, null, null, "1.2.3", "[vV]")]
        [TestCase("V1.2.3", 1, 2, 3, null, null, null, null, null, null, "1.2.3", "[vV]")]
        [TestCase("version-1.2.3", 1, 2, 3, null, null, null, null, null, null, "1.2.3", "version-")]
        public void ParseTest(
            string versionString, int major, int minor, int patch, string tag, int? tagNumber, int? numberOfBuilds,
            string branchName, string sha, string otherMetaData, string fullFormattedVersionString, string tagPrefixRegex)
        {
            fullFormattedVersionString = fullFormattedVersionString ?? versionString;
            var result = SemanticVersion.TryParse(versionString, tagPrefixRegex, out var version);
            
            Assert.That(result, Is.True, versionString);
            Assert.That(version.Major, Is.EqualTo(major));
            Assert.That(version.Minor, Is.EqualTo(minor));
            Assert.That(version.Patch, Is.EqualTo(patch));
            Assert.That(version.PreReleaseTag.Name, Is.EqualTo(tag));
            Assert.That(version.PreReleaseTag.Number, Is.EqualTo(tagNumber));
            Assert.That(version.BuildMetadata.CommitsSinceTag, Is.EqualTo(numberOfBuilds));
            Assert.That(version.BuildMetadata.Branch, Is.EqualTo(branchName));
            Assert.That(version.BuildMetadata.Sha, Is.EqualTo(sha));
            Assert.That(version.BuildMetadata.OtherMetaData, Is.EqualTo(otherMetaData));
            Assert.That(version.ToString("i"), Is.EqualTo(fullFormattedVersionString));
        }

        [Test]
        [TestCase("1.0.0", "1.1.0")]
        [TestCase("1.0.0", "1.0.1")]
        [TestCase("1.0.0", "1.0.1-beta.1+4.Sha.12234.Othershiz")]
        public void CompareTest_Less(string leftInput, string rightInput)
        {
            var left = SemanticVersion.Parse(leftInput, null);
            var right = SemanticVersion.Parse(rightInput, null);
            
            Assert.That(left, Is.LessThan(right));
        }
        
        [Test]
        [TestCase("1.1.0", "1.0.0")]
        [TestCase("1.0.1", "1.0.0")]
        [TestCase("1.0.0-beta.1+4.Sha.12234.Othershiz", "1.0.0")]
        public void CompareTest_Greater(string leftInput, string rightInput)
        {
            var left = SemanticVersion.Parse(leftInput, null);
            var right = SemanticVersion.Parse(rightInput, null);
            
            Assert.That(left, Is.GreaterThan(right));
        }
        
        [Test]
        [TestCase("1.0.0", "1.0.0")]
        public void CompareTest_Equal(string leftInput, string rightInput)
        {
            var left = SemanticVersion.Parse(leftInput, null);
            var right = SemanticVersion.Parse(rightInput, null);
            
            Assert.That(left, Is.EqualTo(right));
        }
    }
}