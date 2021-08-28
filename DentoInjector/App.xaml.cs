using System.Windows;
using System.Windows.Threading;
using DentoInjector.Core;
using DentoInjector.Graphics;
using AdonisMessageBox = AdonisUI.Controls.MessageBox;

namespace DentoInjector
{

    public partial class App
    {

        internal static Configuration Settings { get; } = Settings = Configuration.Load();

        private void InitializeApp(object sender, StartupEventArgs args)
        {
            if (!Utilities.IsRunningAsAdministrator())
            {
                AdonisMessageBox.Show("You need to run this program as administrator to use it!", "DentoInjector");
                Current.Shutdown();
            }
            MainWindow = new WnMain();
            MainWindow.Show();
        }

        private void HandleException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            new WnExceptionHandler(args.Exception).ShowDialog();
        }

    }

}