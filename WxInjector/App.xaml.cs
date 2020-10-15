using System.Windows;
using System.Windows.Threading;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using WxInjector.Core;
using WxInjector.Graphics;
using AdonisMessageBox = AdonisUI.Controls.MessageBox;

namespace WxInjector
{

    public partial class App
    {

        internal static Configuration Settings { get; private set; }

        private void InitializeApp(object sender, StartupEventArgs args)
        {
            AppCenter.Start("12cb4cef-c869-46d8-86a7-b8d1efe2835e", typeof(Analytics), typeof(Crashes));
            Settings = Configuration.Load();
            if (!Utilities.IsRunningAsAdministrator())
            {
                AdonisMessageBox.Show("You need to run this program as administrator to use it!", "WxInjector");
                Current.Shutdown();
            }
            MainWindow = new WnMain();
            MainWindow.Show();
        }

        private void HandleError(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            #if !DEBUG
            Crashes.TrackError(args.Exception);
            #endif
            new WnErrorHandler(args.Exception).ShowDialog();
        }

    }

}