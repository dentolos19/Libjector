using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace WxInjector.Core.Models
{

    [Serializable]
    public class Records
    {

        private static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WxInjector.cfg");
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Records));

        public string[] DLLs = null;

        public void Save()
        {
            var stream = new StreamWriter(Source);
            Serializer.Serialize(stream, this);
            stream.Close();
        }

        [SuppressMessage("Security", "CA3075")]
        [SuppressMessage("Security", "CA5369")]
        public static Records Load()
        {
            var result = new Records();
            if (!File.Exists(Source))
                return result;
            var stream = new StreamReader(Source);
            result = Serializer.Deserialize(stream) as Records;
            stream.Close();
            return result;
        }

    }

}