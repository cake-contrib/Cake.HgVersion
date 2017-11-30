using NUnit.Framework;
using VCSVersion;
using VCSVersion.SemanticVersions;

namespace HgVersionTests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class VersionEngineTests
    {
        [Test]
        public void ExecuteTest_OnlyInitedRepository()
        {
            using (var context = new TestVesionContext())
            {
                var engine = new VersionEngine(context);
                var version = engine.Execute();
                var expected = SemanticVersion.Parse("0.1.0");
                var comparer = new SemanticVersionComarer(SemanticVersionComparation.MajorMinorPatch);
                
                Assert.That(version, Is.EqualTo(expected).Using(comparer));
            }
        }
        
        [Test]
        public void ExecuteTest()
        {
            using (var context = new TestVesionContext())
            {
                context.WriteTextAndCommit("test.txt", "dummy");
                
                var engine = new VersionEngine(context);
                var version = engine.Execute();
                var expected = SemanticVersion.Parse("0.1.0");
                var comparer = new SemanticVersionComarer(SemanticVersionComparation.MajorMinorPatch);
                
                Assert.That(version, Is.EqualTo(expected).Using(comparer));
            }
        }
    }
}
