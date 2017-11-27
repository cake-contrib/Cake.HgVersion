using HgVersion;
using HgVersion.SemanticVersions;
using HgVersion.VCS;
using HgVersion.VersionCalculation;
using HgVersion.VersionCalculation.BaseVersionCalculation;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace HgVersionTests.VersionCalculation
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class NextVersionCalculatorTests
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }
        
        [Test]
        public void CalculateNextVersion()
        {
            var baseCalculatorMock = _fixture.Create<Mock<IBaseVersionCalculator>>();
            baseCalculatorMock.Setup(c => c.CalculateVersion(It.IsAny<IVersionContext>()))
                .Returns(
                    new BaseVersion("", new SemanticVersion(1), null, true));

            var metadatCalculatorMock = _fixture.Create<Mock<IMetadataCalculator>>();
            metadatCalculatorMock.Setup(m => m.CalculateMetadata(
                    It.IsAny<IVersionContext>(),
                    It.IsAny<ICommit>()))
                .Returns((BuildMetadata) null);
            
            var tagCalculatorMock = _fixture.Create<Mock<IPreReleaseTagCalculator>>();
            tagCalculatorMock.Setup(t => t.CalculateTag(
                    It.IsAny<IVersionContext>(),
                    It.IsAny<SemanticVersion>()))
                .Returns((PreReleaseTag)null);
            
            var baseCalculator = baseCalculatorMock.Object;
            var metadataCalculator = metadatCalculatorMock.Object;
            var tagCalculator = tagCalculatorMock.Object;
            
            var versionCalculator = new NextVersionCalculator(
                baseCalculator,
                metadataCalculator,
                tagCalculator);
            
            var version = versionCalculator.CalculateVersion(It.IsAny<IVersionContext>());
            var expected = SemanticVersion.Parse("1.0.1");
            
            Assert.That(version, Is.EqualTo(expected));
        }
    }
}
