namespace WxInjector.Core.Bindings
{

    public class DllFileBinding
    {

        public string Name { get; set; }

        public string Architecture { get; set; }

        public string Path { get; set; }

        public static DllFileBinding Create(string dllPath)
        {
            return new DllFileBinding
            {
                Path = dllPath,
                Name = System.IO.Path.GetFileName(dllPath),
                Architecture = Utilities.GetDllArchitecture(dllPath)
            };
        }

    }

}