using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bleak;
using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using WxInjector.Core;
using WxInjector.Core.Bindings;

namespace WxInjector.Graphics
{

    public partial class WnMain
    {

        private int _targetProcessId;
        private Injector _currentInjector;

        public WnMain()
        {
            InitializeComponent();
            foreach (var color in ThemeManager.Current.ColorSchemes)
                ColorSchemeBox.Items.Add(color);
            ColorSchemeBox.Text = App.Settings.ColorScheme;
            if (App.Settings.DllFiles == null)
                return;
            foreach (var dll in App.Settings.DllFiles)
                DllFileList.Items.Add(dll);
            UpdateDllSelection(null, null);
        }

        private void ColorSchemeSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            App.Settings.ColorScheme = (string)((ComboBox)sender).SelectedItem;
            Utilities.SetAppTheme(App.Settings.ColorScheme, true);
            App.Settings.Save();
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private async void Inject(object sender, RoutedEventArgs args)
        {
            if (!InjectButton.IsEnabled)
                return;
            if (DllFileList.SelectedItem == null || _targetProcessId <= 0)
            {
                await this.ShowMessageAsync("Needs additional input!", "Select a DLL and a target process before continuing.").ConfigureAwait(false);
                return;
            }
            try
            {
                var binding = (DllFileBinding)DllFileList.SelectedItem;
                InjectionMethod method;
                switch (MethodBox.SelectedIndex)
                {
                    case 1:
                        method = InjectionMethod.HijackThread;
                        break;
                    case 2:
                        method = InjectionMethod.ManualMap;
                        break;
                    default:
                        method = InjectionMethod.CreateThread;
                        break;
                }
                InjectionFlags flag;
                switch (FlagBox.SelectedIndex)
                {
                    case 1:
                        flag = InjectionFlags.HideDllFromPeb;
                        break;
                    case 2:
                        flag = InjectionFlags.RandomiseDllHeaders;
                        break;
                    case 3:
                        flag = InjectionFlags.RandomiseDllName;
                        break;
                    default:
                        flag = InjectionFlags.None;
                        break;
                }
                _currentInjector = new Injector(_targetProcessId, binding.Path, method, flag);
                _currentInjector.InjectDll();
                if (flag != InjectionFlags.HideDllFromPeb)
                {
                    InjectButton.IsEnabled = false;
                    EjectButton.IsEnabled = true;
                }
                await this.ShowMessageAsync("Injection successful!", "DLL has been injected into process!").ConfigureAwait(false);
            }
            catch
            {
                await this.ShowMessageAsync("Injection unsuccessful!", "DLL has been injected into process! Restart and reselect the target process and try again.").ConfigureAwait(false);
            }
        }

        private async void Eject(object sender, RoutedEventArgs args)
        {
            if (!EjectButton.IsEnabled)
                return;
            try
            {
                _currentInjector.EjectDll();
                _currentInjector.Dispose();
                await this.ShowMessageAsync("Ejection successful!", "DLL has been ejected from process!").ConfigureAwait(false);
            }
            catch
            {
                await this.ShowMessageAsync("Ejection unsuccessful!", "Unable to eject from process!").ConfigureAwait(false);
            }
            InjectButton.IsEnabled = true;
            EjectButton.IsEnabled = false;
        }

        private void Add(object sender, RoutedEventArgs args)
        {
            var dialog = new OpenFileDialog { Filter = "Dynamic Link Library|*.dll" };
            if (dialog.ShowDialog() != true)
                return;
            var binding = DllFileBinding.Create(dialog.FileName);
            DllFileList.Items.Add(binding);
            App.Settings.DllFiles = DllFileList.Items.OfType<DllFileBinding>().ToArray();
            UpdateDllSelection(null, null);
        }

        private void Remove(object sender, RoutedEventArgs args)
        {
            if (DllFileList.SelectedItem == null)
                return;
            DllFileList.Items.Remove(DllFileList.SelectedItem);
            App.Settings.DllFiles = DllFileList.Items.OfType<DllFileBinding>().ToArray();
            UpdateDllSelection(null, null);
        }

        private void Clear(object sender, RoutedEventArgs args)
        {
            DllFileList.Items.Clear();
            App.Settings.DllFiles = DllFileList.Items.OfType<DllFileBinding>().ToArray();
            UpdateDllSelection(null, null);
        }

        private void SelectProcess(object sender, RoutedEventArgs args)
        {
            var dialog = new WnSelectProcess { Owner = this };
            if (dialog.ShowDialog() == false)
                return;
            _targetProcessId = dialog.SelectedProcessId;
            TargetProcessBox.Text = $"{dialog.SelectedProcessName} ({dialog.SelectedProcessId})";
        }

        private void SaveSettings(object sender, CancelEventArgs args)
        {
            App.Settings.Save();
        }

        private void UpdateDllSelection(object sender, SelectionChangedEventArgs args)
        {
            RemoveButton.IsEnabled = DllFileList.SelectedItem != null;
            ClearButton.IsEnabled = DllFileList.Items.Count > 0;
        }

    }

}