using System;
using System.Windows;
using DentoInjector.Core;

namespace DentoInjector.Graphics
{

    public partial class WnExceptionHandler
    {

        public WnExceptionHandler(Exception error)
        {
            InitializeComponent();
            MessageText.Text = error.Message;
            StackTraceText.Text = error.StackTrace;
        }

        private void Restart(object sender, RoutedEventArgs args)
        {
            Utilities.RestartApp();
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

    }

}