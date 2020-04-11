using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using WxInjector.Core;
using WxInjector.Graphics;

namespace WxInjector
{

    public static class Program
    {

        [STAThread]
        [SuppressMessage("Reliability", "CA2000")]
        [SuppressMessage("Globalization", "CA1303")]
        [SuppressMessage("Globalization", "CA1305")]
        [SuppressMessage("Design", "CA1031")]
        public static void Main()
        {
            var arguments = Environment.GetCommandLineArgs();
            if (arguments.Length == 2)
            {
                Native.AllocConsole();
                Console.WriteLine(@"[WxInjector] Injecting DLL into Process...");
                var result = Injector.Result.InjectionSuccessful;
                try
                {
                    var injector = new Injector(int.Parse(arguments[0]));
                    result = injector.Inject(arguments[1]);
                    injector.Dispose();
                }
                catch (Exception error)
                {
                    Console.WriteLine(@"[WxInjector] An error had occurred! {0}", error.Message);
                }
                if (result != Injector.Result.InjectionSuccessful)
                {
                    switch (result)
                    {
                        case Injector.Result.SystemProcessDisallowed:
                            Console.WriteLine(@"[WxInjector] Injecting into system process is not allowed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingProcessHandleFailed:
                            Console.WriteLine(@"[WxInjector] Obtaining process handle failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.DLLAllocationFailed:
                            Console.WriteLine(@"[WxInjector] DLL allocation failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.DLLWritingFailed:
                            Console.WriteLine(@"[WxInjector] DLL writing failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingLoaderAddressFailed:
                            Console.WriteLine(@"[WxInjector] Obtaining loader address failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingThreadHandleFailed:
                            Console.WriteLine(@"[WxInjector] Obtaining thread handle failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.UnableReleaseMemory:
                            Console.WriteLine(@"[WxInjector] Unable to release memory! Injection might be successful!");
                            break;
                    }
                    return;
                }
                Console.WriteLine(@"[WxInjector] Injection successful!");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new WnMain());
            }
        }

    }

}