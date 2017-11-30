using System;
using NUnit.Framework;
using VCSVersion.SemanticVersions;

namespace VCSVersionTests.SemanticVersions
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
        
        [TestCase("feature1", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "unstable", 1, "1.2.3-unstable+1.Branch.feature1.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("develop", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "alpha645", null, "1.2.3-alpha.645+Branch.develop.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("develop", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "unstable645", null, "1.2.3-unstable.645+Branch.develop.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("develop", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "beta645", null, "1.2.3-beta.645+Branch.develop.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("hotfix-foo", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "alpha645", null, "1.2.3-alpha.645+Branch.hotfix-foo.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("hotfix-foo", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "beta645", null, "1.2.3-beta.645+Branch.hotfix-foo.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("hotfix-foo", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, null, null, "1.2.3+Branch.hotfix-foo.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("master", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, null, null, "1.2.3+Branch.master.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("myPullRequest", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 3, "unstable3", null, "1.2.3-unstable.3+Branch.myPullRequest.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("release-1.2", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 0, "beta2", null, "1.2.0-beta.2+Branch.release-1.2.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        [TestCase("release-1.2", "a682956dc1a2752aa24597a0f5cd939f93614509", 1, 2, 0, "alpha2", null, "1.2.0-alpha.2+Branch.release-1.2.Sha.a682956dc1a2752aa24597a0f5cd939f93614509")]
        public void ValidateInformationalVersionBuilder(string branchName, string hash, int major, int minor, int patch,
            string tag, int? suffix, string versionString)
        {
            var semanticVersion = new SemanticVersion
            (
                major,
                minor,
                patch,
                tag,
                new BuildMetadata(suffix, branchName, hash, DateTimeOffset.MinValue)
            );
            var informationalVersion = semanticVersion.ToString("i");
    
            Assert.AreEqual(versionString, informationalVersion);
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