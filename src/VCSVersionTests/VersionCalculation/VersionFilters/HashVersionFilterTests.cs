using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;
using VCSVersion.VersionCalculation.BaseVersionCalculation;
using VCSVersion.VersionFilters;

namespace VCSVersionTests.VersionCalculation.VersionFilters
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class HashVersionFilterTests
    {
        private IFixture _fixture;

        public HashVersionFilterTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void VerifyNullGuard()
        {
            Assert.Throws<ArgumentNullException>(() => new HashVersionFilter(null));
        }

        [Test]
        public void VerifyNullGuard2()
        {
            var commitMock = _fixture.Create<Mock<ICommit>>();
            commitMock.Setup(c => c.Hash)
                .ReturnsUsingFixture(_fixture);

            var commit = commitMock.Object;
            var sut = new HashVersionFilter(new[] { commit.Hash });

            Assert.Throws<ArgumentNullException>(() => sut.Exclude(null, out var reason));
        }

        [Test]
        public void WhenHashMatchShouldExcludeWithReason()
        {
            var commitMock = _fixture.Create<Mock<ICommit>>();
            commitMock.Setup(c => c.Hash)
                .ReturnsUsingFixture(_fixture);

            var commit = commitMock.Object;
            var version = new BaseVersion("dummy", new SemanticVersion(1), commit, false);
            var sut = new HashVersionFilter(new[] { commit.Hash });

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.True);
            Assert.That(reason, Is.Not.Empty);
        }

        [Test]
        public void WhenHashMismatchShouldNotExclude()
        {
            var commitMock = _fixture.Create<Mock<ICommit>>();
            commitMock.Setup(c => c.Hash)
                .ReturnsUsingFixture(_fixture);

            var commit = commitMock.Object;
            var version = new BaseVersion("dummy", new SemanticVersion(1), commit, false);
            var sut = new HashVersionFilter(new[] { "mismatched" });

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.Null);
        }

        [Test]
        public void ExcludeShouldAcceptVersionWithNullCommit()
        {
            var version = new BaseVersion("dummy", new SemanticVersion(1), null, false);
            var sut = new HashVersionFilter(new[] { "mismatched" });

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.Null);
        }
    }
}
