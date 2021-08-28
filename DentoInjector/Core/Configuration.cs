using System;
using System.IO;
using System.Xml.Serialization;
using DentoInjector.Core.Bindings;

namespace DentoInjector.Core
{

    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DentoInjector.cfg");
        private static readonly XmlSerializer Serializer = new(typeof(Configuration));

        public DllFileBinding[]? DllFiles { get; set; }

        public void Save()
        {
            using var stream = new FileStream(Source, FileMode.Create);
            Serializer.Serialize(stream, this);
        }

        public static Configuration Load()
        {
            if (!File.Exists(Source))
                return new Configuration();
            using var stream = new FileStream(Source, FileMode.Open);
            return (Configuration)Serializer.Deserialize(stream)!;
        }

    }

}