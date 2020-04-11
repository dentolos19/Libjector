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
            var Arguments = Environment.GetCommandLineArgs();
            if (Arguments.Length == 2)
            {
                Native.AllocConsole();
                Console.WriteLine("[WxInjector] Injecting DLL into Process...");
                var Result = Injector.Result.InjectionSuccessful;
                try
                {
                    var Injector = new Injector(int.Parse(Arguments[0]));
                    Result = Injector.Inject(Arguments[1]);
                    Injector.Dispose();
                }
                catch (Exception Error)
                {
                    Console.WriteLine("[WxInjector] An error had occurred! {0}", Error.Message);
                }
                if (Result != Injector.Result.InjectionSuccessful)
                {
                    switch (Result)
                    {
                        case Injector.Result.SystemProcessDisallowed:
                            Console.WriteLine("[WxInjector] Injecting into system process is not allowed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingProcessHandleFailed:
                            Console.WriteLine("[WxInjector] Obtaining process handle failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.DLLAllocationFailed:
                            Console.WriteLine("[WxInjector] DLL allocation failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.DLLWritingFailed:
                            Console.WriteLine("[WxInjector] DLL writing failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingLoaderAddressFailed:
                            Console.WriteLine("[WxInjector] Obtaining loader address failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.ObtainingThreadHandleFailed:
                            Console.WriteLine("[WxInjector] Obtaining thread handle failed! Injection unsuccessful!");
                            break;
                        case Injector.Result.UnableReleaseMemory:
                            Console.WriteLine("[WxInjector] Unable to release memory! Injection might be successful!");
                            break;
                    }
                    return;
                }
                Console.WriteLine("[WxInjector] Injection successful!");
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