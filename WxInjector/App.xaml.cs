using System.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WxInjector.Core;
using WxInjector.Graphics;

namespace WxInjector
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        private void Initialize(object sender, StartupEventArgs args)
        {
            AppCenter.Start("12cb4cef-c869-46d8-86a7-b8d1efe2835e", typeof(Analytics), typeof(Crashes));
            Settings = Configuration.Load();
            if (!Utilities.IsRunningAsAdministrator())
            {
                MessageBox.Show("You need to run this program as administrator to use it!", "WxInjector");
                Current.Shutdown();
            }
            new WnMain().Show();
        }

    }

}