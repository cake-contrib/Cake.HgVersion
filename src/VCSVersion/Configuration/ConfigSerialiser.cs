using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VCSVersion.Configuration
{
    public class ConfigSerialiser
    {
        public static Config Read(TextReader reader)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();

            return deserializer.Deserialize<Config>(reader) ?? new Config();
        }

        public static void Write(Config config, TextWriter writer)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();

            serializer.Serialize(writer, config);
        }
    }
}
