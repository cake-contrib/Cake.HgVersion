using NUnit.Framework;

namespace HgVersionTests.IntegrationTests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class DevelopScenarios
    {
        [Test]
        public void WhenDevelopHasMultipleCommits_SpecifyExistingCommitId()
        {
            using (var context = new TestVesionContext())
            {
                context.CreateBranch("dev");
                context.WriteTextAndCommit("dummy.txt", "", "init commit");
                context.MakeTaggedCommit("1.0.0");

                context.MakeCommit();
                context.MakeCommit();
                context.MakeCommit();

                var thirdCommit = context.Tip(); 
                context.MakeCommit();
                context.MakeCommit();

                context.AssertFullSemver("1.1.0-alpha.3", commitId: thirdCommit.Hash);
            }
        }
        
        [Test]
        public void WhenDevelopBranchedFromTaggedCommitOnDefaultVersionDoesNotChange()
        {
            using (var context = new TestVesionContext())
            {
                context.CreateBranch("dev");
                context.WriteTextAndCommit("dummy.txt", "", "init commit");
                context.MakeTaggedCommit("1.0.0");
                context.AssertFullSemver("1.0.0");
            }
        }
    }
}