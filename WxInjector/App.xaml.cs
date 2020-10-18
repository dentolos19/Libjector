using System.Windows;
using System.Windows.Threading;
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
            new WnErrorHandler(args.Exception).ShowDialog();
        }

    }

}