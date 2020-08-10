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
            Utilities.SetAppTheme(Settings.ColorScheme, true, false);
            new WnMain().Show();
        }

    }

}