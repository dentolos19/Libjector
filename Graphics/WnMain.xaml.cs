using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Bleak;
using Microsoft.Win32;
using WxInjector.Core;

namespace WxInjector.Graphics
{

    public partial class WnMain
    {

        private int _target = -1;

        public WnMain()
        {
            InitializeComponent();
            RefreshProcesses(null, null);
        }

        private void RefreshProcesses(object sender, RoutedEventArgs e)
        {
            TbProcess.Text = string.Empty;
            LbProcesses.Items.Clear();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle))
                    continue;

                var item = new ListBoxItem { Content = $"{process.ProcessName}.exe ({process.Id})", Tag = process.Id.ToString() };
                LbProcesses.Items.Add(item);
            }
        }

        private void BrowseLibrary(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Title = "WxInjector Library Opener", Filter = "Dynamic Link Library|*.dll" };
            if (openDialog.ShowDialog() == true)
                TbLibrary.Text = openDialog.FileName;
        }

        private void ProcessSelected(object sender, SelectionChangedEventArgs e)
        {
            if (LbProcesses.SelectedItem == null)
                return;
            var item = LbProcesses.SelectedItem as ListBoxItem;
            TbProcess.Text = item.Content.ToString();
            _target = int.Parse(item.Tag.ToString());
        }

        private void Inject(object sender, RoutedEventArgs e)
        {
            if (_target < 0)
            {
                MessageBox.Show("Select a process before injecting!", "WxInjector");
                return;
            }
            if (TbLibrary.Text.Length == 0)
            {
                MessageBox.Show("Select a library before injecting!", "WxInjector");
                return;
            }
            var method = InjectionMethod.CreateThread;
            var flag = InjectionFlags.None;
            method = CbMethod.SelectedIndex switch
            {
                0 => InjectionMethod.CreateThread,
                1 => InjectionMethod.HijackThread,
                2 => InjectionMethod.ManualMap,
                _ => method
            };
            flag = CbFlag.SelectedIndex switch
            {
                0 => InjectionFlags.None,
                1 => InjectionFlags.HideDllFromPeb,
                2 => InjectionFlags.RandomiseDllHeaders,
                3 => InjectionFlags.RandomiseDllName,
                _ => flag
            };
            var bytes = File.ReadAllBytes(TbLibrary.Text);
            var injector = new Injector(_target, bytes, method, flag);
            injector.InjectDll();
            injector.Dispose();
        }

        private void CheckForUpdates(object sender, RoutedEventArgs e)
        {
            if (!Utilities.IsUserOnline())
            {
                MessageBox.Show("An internet connection is required for this operation!", "WxInjector");
                return;
            }
            if (Utilities.IsUpdateAvailable())
            {
                var result = MessageBox.Show("Updates is available! Do you want to visit the download page?", @"WxInjector", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    Process.Start("https://github.com/dentolos19/WxInjector/releases");
            }
            else
            {
                MessageBox.Show("No updates is available, keep doing your thing!", "WxInjector");
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }

}