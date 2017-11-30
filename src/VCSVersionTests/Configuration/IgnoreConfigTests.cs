using NUnit.Framework;
using System;
using System.IO;
using VCSVersion.Configuration;
using YamlDotNet.Core;

namespace VCSVersionTests.Configuration
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class IgnoreConfigTests
    {
        [Test]
        public void CanDeserialize()
        {
            var yaml = @"
            ignore:
                sha: [b6c0c9fda88830ebcd563e500a5a7da5a1658e98]
                commits-before: 2015-10-23T12:23:15
            ";

            using (var reader = new StringReader(yaml))
            {
                var config = ConfigSerialiser.Read(reader);

                Assert.That(config.Ignore, Is.Not.Null);
                Assert.That(config.Ignore.Hashes, Is.Not.Empty);
                Assert.That(config.Ignore.Hashes, Is.EqualTo(new[] { "b6c0c9fda88830ebcd563e500a5a7da5a1658e98" }));
                Assert.That(config.Ignore.Before, Is.EqualTo(DateTimeOffset.Parse("2015-10-23T12:23:15")));
            }
        }

        [Test]
        public void ShouldSupportsOtherSequenceFormat()
        {
            var yaml = @"
            ignore:
                sha: 
                    - b6c0c9fda88830ebcd563e500a5a7da5a1658e98
                    - 6c19c7c219ecf8dbc468042baefa73a1b213e8b1
            ";

            using (var reader = new StringReader(yaml))
            {
                var config = ConfigSerialiser.Read(reader);

                Assert.That(config.Ignore, Is.Not.Null);
                Assert.That(config.Ignore.Hashes, Is.Not.Empty);
                Assert.That(config.Ignore.Hashes, Is.EqualTo(new[] { "b6c0c9fda88830ebcd563e500a5a7da5a1658e98", "6c19c7c219ecf8dbc468042baefa73a1b213e8b1" }));
            }
        }

        [Test]
        public void WhenNotInConfigShouldHaveDefaults()
        {
            var yaml = @"
            next-version: 1.0
            ";

            using (var reader = new StringReader(yaml))
            {
                var config = ConfigSerialiser.Read(reader);

                Assert.That(config.Ignore, Is.Not.Null);
                Assert.That(config.Ignore.Hashes, Is.Empty);
                Assert.That(config.Ignore.Before, Is.Null);
            }
        }

        [Test]
        public void WhenBadDateFormatShouldFail()
        {
            var yaml = @"
            ignore:
                commits-before: bad format date
            ";

            using (var reader = new StringReader(yaml))
            {
                Assert.Throws<YamlException>(() => ConfigSerialiser.Read(reader));
            }
        }
    }
}
