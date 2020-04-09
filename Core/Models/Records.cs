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
            var Stream = new FileStream(Source, FileMode.Create);
            Formatter.Serialize(Stream, this);
            Stream.Close();
        }

        public static Records Load()
        {
            var Result = new Records();
            if (File.Exists(Source))
            {
                var Stream = new FileStream(Source, FileMode.Open);
                Result = Formatter.Deserialize(Stream) as Records;
                Stream.Close();
            }

            return Result;
        }
    }
}