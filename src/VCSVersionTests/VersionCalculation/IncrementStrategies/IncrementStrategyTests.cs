using NUnit.Framework;
using VCSVersion.SemanticVersions;
using VCSVersion.VersionCalculation;

namespace VCSVersionTests.VersionCalculation
{
    [TestFixture]
    public class IncrementStrategyTests
    {
        [TestCase("1.0.0", "1.0.1", VersionField.Patch)]
        [TestCase("1.0.0", "1.1.0", VersionField.Minor)]
        [TestCase("1.0.0", "2.0.0", VersionField.Major)]
        [Test]
        public void IncrementVersion_VersionFieldTest(string source, string expected, VersionField versionField)
        {
            var sourceVer = SemanticVersion.Parse(source);
            var expectedVer = SemanticVersion.Parse(expected);
            
            var strategy = new IncrementStrategy(versionField);
            var actualVer = strategy.IncrementVersion(sourceVer);
            
            Assert.That(actualVer, Is.EqualTo(expectedVer));
        }
    }
}