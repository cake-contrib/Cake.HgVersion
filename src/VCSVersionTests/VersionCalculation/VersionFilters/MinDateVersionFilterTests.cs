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
    public class MinDateVersionFilterTests
    {
        private IFixture _fixture;

        public MinDateVersionFilterTests()
        {
            _fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
        }

        [Test]
        public void VerifyNullGuard()
        {
            var dummy = DateTimeOffset.UtcNow.AddSeconds(1.0);
            var sut = new MinDateVersionFilter(dummy);

            string reason;
            Assert.Throws<ArgumentNullException>(() => sut.Exclude(null, out reason));
        }

        [Test]
        public void WhenCommitShouldExcludeWithReason()
        {
            var commitMock = _fixture.Create<Mock<ICommit>>();
            commitMock.Setup(c => c.When)
                .Returns(DateTime.UtcNow);

            var commit = commitMock.Object;
            var version = new BaseVersion("dummy", new SemanticVersion(1), commit);
            var futureDate = DateTimeOffset.UtcNow.AddYears(1);
            var sut = new MinDateVersionFilter(futureDate);

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.True);
            Assert.That(reason, Is.Not.Empty);
        }

        [Test]
        public void WhenShaMismatchShouldNotExclude()
        {
            var commitMock = _fixture.Create<Mock<ICommit>>();
            commitMock.Setup(c => c.When)
                .Returns(DateTime.UtcNow);

            var commit = commitMock.Object;
            var version = new BaseVersion("dummy", new SemanticVersion(1), commit);
            var pastDate = DateTimeOffset.UtcNow.AddYears(-1);
            var sut = new MinDateVersionFilter(pastDate);

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.Null);
        }

        [Test]
        public void ExcludeShouldAcceptVersionWithNullCommit()
        {
            var version = new BaseVersion("dummy", new SemanticVersion(1), null);
            var futureDate = DateTimeOffset.UtcNow.AddYears(1);
            var sut = new MinDateVersionFilter(futureDate);

            var result = sut.Exclude(version, out var reason);

            Assert.That(result, Is.False);
            Assert.That(reason, Is.Null);
        }
    }
}
