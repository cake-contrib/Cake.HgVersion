using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using VCSVersion;
using VCSVersion.SemanticVersions;
using VCSVersion.VCS;
using VCSVersion.VersionCalculation.BaseVersionCalculation;
using VCSVersionTests.Configuration;

namespace VCSVersionTests.VersionCalculation.BaseVersionCalculation
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class MergeMessageBaseVersionStrategyTests
    {
        [Test]
        [Description("When a branch is merged in you want to start building stable packages of that version. So we shouldn't bump the version.")]
        public void ShouldNotAllowIncrementOfVersion()
        {
            var parents = GetParents(true);
            var baseVersion = GetBaseVersion(parents, preventIncrementForMergedBranchVersion: true);

            baseVersion.ShouldIncrement.ShouldBe(false);
        }
        
        [Test]
        public void ShouldReturnBaseVersionForMergeCommit()
        {
            var parents = GetParents(isMergeCommit: true);
            var baseVersion = GetBaseVersion(parents);

            baseVersion.ShouldNotBeNull();
        }

        [Test]
        public void ShouldNotReturnBaseVersionForNonMergeCommit()
        {
            var parents = GetParents(isMergeCommit: false);
            var baseVersion = GetBaseVersion(parents);

            baseVersion.ShouldBeNull();
        }

        private static BaseVersion GetBaseVersion(List<ICommit> parents, bool preventIncrementForMergedBranchVersion = false)
        {
            var commitMock = new Mock<ICommit>();
            var commit = commitMock.Object;
            commitMock.Setup(c => c.When).Returns(DateTime.Now);
            
            var repositoryMock = new Mock<IRepository>();
            var repository = repositoryMock.Object;
            repositoryMock.Setup(r => r.Parents(It.IsAny<ICommit>())).Returns(parents);
            repositoryMock.Setup(r => r.Log(It.IsAny<Func<ILogQueryBuilder, ILogQuery>>()))
                .Returns(Enumerable.Repeat(commit, 1));

            var mergeMessageMock = new Mock<IMergeMessage>();
            var mergeMessage = mergeMessageMock.Object;
            mergeMessageMock.Setup(m => m.Version).Returns(new SemanticVersion());

            var providerMock = new Mock<IRepositoryMetadataProvider>();
            var provider = providerMock.Object;
            providerMock.Setup(p => p.ParseMergeMessage(It.IsAny<string>())).Returns(mergeMessage);

            var contextMock = new Mock<IVersionContext>();
            var context = contextMock.Object;
            contextMock.Setup(c => c.Repository).Returns(repository);
            contextMock.Setup(c => c.CurrentCommit).Returns(commit);
            contextMock.Setup(c => c.CurrentBranch).Returns(Mock.Of<IBranchHead>());
            contextMock.Setup(c => c.Configuration).Returns(new TestEffectiveConfiguration(preventIncrementForMergedBranchVersion: preventIncrementForMergedBranchVersion));
            contextMock.Setup(c => c.RepositoryMetadataProvider).Returns(provider);

            var sut = new MergeMessageBaseVersionStrategy();
            return sut.GetVersions(context).SingleOrDefault();
        }

        private static List<ICommit> GetParents(bool isMergeCommit)
        {
            if (isMergeCommit)
            {
                return new List<ICommit>
                {
                    Mock.Of<ICommit>(),
                    Mock.Of<ICommit>()
                };
            }

            return new List<ICommit> { Mock.Of<ICommit>() };
        }
    }
}
