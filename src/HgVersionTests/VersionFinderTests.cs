using NUnit.Framework;
using VCSVersion;
using VCSVersion.SemanticVersions;

namespace HgVersionTests
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class VersionFinderTests
    {
        //[Test]
        //public void FindVersionTest_OnlyInitedRepository()
        //{
        //    using (var context = new TestVesionContext())
        //    {
        //        var finder = new VersionFinder();
        //        var version = finder.FindVersion(context);
        //        var expected = SemanticVersion.Parse("0.1.0");
        //        var comparer = new SemanticVersionComarer(SemanticVersionComparation.MajorMinorPatch);
                
        //        Assert.That(version, Is.EqualTo(expected).Using(comparer));
        //    }
        //}
        
        //[Test]
        //public void FindVersionTest()
        //{
        //    using (var context = new TestVesionContext())
        //    {
        //        context.WriteTextAndCommit("test.txt", "dummy");

        //        var finder = new VersionFinder();
        //        var version = finder.FindVersion(context);
        //        var expected = SemanticVersion.Parse("0.1.0");
        //        var comparer = new SemanticVersionComarer(SemanticVersionComparation.MajorMinorPatch);
                
        //        Assert.That(version, Is.EqualTo(expected).Using(comparer));
        //    }
        //}
    }
}
