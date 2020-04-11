using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WxInjector.Core.Models
{

    [Serializable]
    internal class Records
    {

        public static readonly string Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WxInjector.cfg");
        private static readonly BinaryFormatter Formatter = new BinaryFormatter();

        public string[] DLLs = null;

        public void Save()
        {
            var stream = new FileStream(Source, FileMode.Create);
            Formatter.Serialize(stream, this);
            stream.Close();
        }

        public static Records Load()
        {
            var result = new Records();
            if (!File.Exists(Source))
                return result;
            var stream = new FileStream(Source, FileMode.Open);
            result = Formatter.Deserialize(stream) as Records;
            stream.Close();
            return result;
        }

    }

}