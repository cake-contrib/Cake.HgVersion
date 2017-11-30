using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using VCSVersion;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;
using VCSVersion.VersionCalculation.BaseVersionCalculation;

namespace VCSVersionTests.VersionCalculation.BaseVersionCalculation
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class FallbackBaseVersionStrategyTests
    {
        private IFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }
        
        [Test]
        public void GetVersionsTest()
        {
            var tipMock = _fixture.Create<Mock<ICommit>>();
            tipMock.Setup(c => c.Hash)
                .ReturnsUsingFixture(_fixture);
            
            var repositoryMock = _fixture.Create<Mock<IRepository>>();
            repositoryMock.Setup(r => r.Tip())
                .Returns(tipMock.Object);
            repositoryMock.Setup(r => r.Log(It.IsAny<Func<ILogQueryBuilder, ILogQuery>>()))
                .Returns(Enumerable.Repeat(tipMock.Object, 1));

            var contextMock = _fixture.Create<Mock<IVersionContext>>();
            contextMock.Setup(c => c.Repository)
                .Returns(repositoryMock.Object);
            
            var tip = tipMock.Object;
            var context = contextMock.Object;
            
            var strategy = new FallbackBaseVersionStrategy();
            var result = strategy.GetVersions(context)
                .ToList();
            
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Source.Hash, Is.EqualTo(tip.Hash));
            Assert.That(result.First().Version, Is.EqualTo(new SemanticVersion(0, 1, 0)));
        }
    }
}