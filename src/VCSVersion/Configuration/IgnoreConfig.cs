using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCSVersion.VersionFilters;
using YamlDotNet.Serialization;

namespace VCSVersion.Configuration
{
    public class IgnoreConfig
    {
        public IgnoreConfig()
        {
            Hashes = Enumerable.Empty<string>();
        }

        [YamlMember(Alias = "commits-before")]
        public DateTimeOffset? Before { get; set; }

        [YamlMember(Alias = "sha")]
        public IEnumerable<string> Hashes { get; set; }

        public virtual IEnumerable<IVersionFilter> ToFilters()
        {
            if (Hashes.Any()) yield return new HashVersionFilter(Hashes);
            if (Before.HasValue) yield return new MinDateVersionFilter(Before.Value);
        }
    }
}
