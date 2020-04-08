using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using WxInjector.Graphics;

namespace WxInjector
{

    public static class Program
    {

        [STAThread]
        [SuppressMessage("Reliability", "CA2000")]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WnMain());
        }

    }

}