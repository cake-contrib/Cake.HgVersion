using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VCSVersion.Helpers;
using YamlDotNet.Serialization;

namespace VCSVersion.Output
{
    public sealed class VersionVariables : IEnumerable<KeyValuePair<string, string>>
    {
        public string Major { get; internal set; }
        public string Minor { get; internal set; }
        public string Patch { get; internal set; }
        public string PreReleaseTag { get; internal set; }
        public string PreReleaseTagWithDash { get; internal set; }
        public string PreReleaseLabel { get; internal set; }
        public string PreReleaseNumber { get; internal set; }
        public string BuildMetadata { get; internal set; }
        public string BuildMetadataPadded { get; internal set; }
        public string FullBuildMetadata { get; internal set; }
        public string MajorMinorPatch { get; internal set; }
        public string SemVer { get; internal set; }
        public string AssemblySemVer { get; internal set; }
        public string AssemblyFileSemVer { get; internal set; }
        public string FullSemVer { get; internal set; }
        public string InformationalVersion { get; internal set; }
        public string BranchName { get; internal set; }
        public string Sha { get; internal set; }
        public string CommitDate { get; internal set; }
        public string NuGetVersion { get; internal set; }
        public string NuGetPreReleaseTag { get; internal set; }
        public string CommitsSinceVersionSource { get; internal set; }

        [ReflectionIgnore]
        public string this[string variable]
        {
            get { return (string)typeof(VersionVariables).GetProperty(variable).GetValue(this, null); }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            var type = typeof(string);
            return typeof(VersionVariables)
                .GetProperties()
                .Where(p => p.PropertyType == type && !p.GetIndexParameters().Any() && !p.GetCustomAttributes(typeof(ReflectionIgnoreAttribute), false).Any())
                .Select(p => new KeyValuePair<string, string>(p.Name, (string)p.GetValue(this, null)))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static VersionVariables FromDictionary(IEnumerable<KeyValuePair<string, string>> properties)
        {
            var type = typeof(VersionVariables);
            var ctor = type.GetConstructors().Single();
            var ctorArgs = ctor.GetParameters()
                .Select(p => properties.Single(v => string.Equals(v.Key, p.Name, StringComparison.CurrentCultureIgnoreCase)).Value)
                .Cast<object>()
                .ToArray();

            return (VersionVariables)Activator.CreateInstance(type, ctorArgs);
        }

        public static VersionVariables FromFile(string filePath, IFileSystem fileSystem)
        {
            using (var stream = fileSystem.OpenRead(filePath))
            using (var reader = new StreamReader(stream))
            {
                var dictionary = new Deserializer()
                    .Deserialize<Dictionary<string, string>>(reader);
                var versionVariables = FromDictionary(dictionary);
                return versionVariables;
            }
        }

        public bool TryGetValue(string variable, out string variableValue)
        {
            if (ContainsKey(variable))
            {
                variableValue = this[variable];
                return true;
            }

            variableValue = null;
            return false;
        }

        public bool ContainsKey(string variable)
        {
            return typeof(VersionVariables).GetProperty(variable) != null;
        }

        public override string ToString()
        {
            return ToString("{0} = {1}");
        }

        public string ToString(string format)
        {
            return string.Join(Environment.NewLine,
                this.Select(v => string.Format(format, v.Key, v.Value)));
        }

        sealed class ReflectionIgnoreAttribute : Attribute
        { }
    }
}