using System.Diagnostics;

namespace WxInjector.Core.Bindings
{

    public class ProcessItemBinding
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Architecture { get; set; }

        public string Path { get; set; }

        public static ProcessItemBinding Create(Process process)
        {
            return new ProcessItemBinding
            {
                Id = process.Id,
                Name = System.IO.Path.GetFileName(process.MainModule!.FileName),
                Architecture = Utilities.GetProcessArchitecture(process),
                Path = process.MainModule.FileName
            };
        }

    }

}