using System;
using System.IO;
using System.Xml.Serialization;

namespace WxInjector.Core
{

    public class Configuration
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "WxInjector.cfg");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Configuration));

        public string ColorScheme { get; set; } = "Cobalt";

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
            return (Configuration)Serializer.Deserialize(stream);
        }

    }

}