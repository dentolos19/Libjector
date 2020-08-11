using System.ComponentModel;
using System.IO;
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
            App.Settings.ColorScheme = ColorSchemeBox.Text;
            Utilities.SetAppTheme(App.Settings.ColorScheme, true);
            App.Settings.Save();
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void Inject(object sender, RoutedEventArgs args)
        {
            if (!InjectButton.IsEnabled)
                return;
            if (DllFileList.SelectedItem == null || _targetProcessId <= 0)
            {
                this.ShowMessageAsync("Needs additional input!", "Select a DLL and a target process before continuing.");
                return;
            }
            var binding = (DllFileBinding)DllFileList.SelectedItem;
            _currentInjector = new Injector(_targetProcessId, binding.Path, InjectionMethod.CreateThread);
            _currentInjector.InjectDll();
            InjectButton.IsEnabled = false;
            EjectButton.IsEnabled = true;
            this.ShowMessageAsync("Injection successful!", "DLL has been injected into process!");
        }

        private void Eject(object sender, RoutedEventArgs args)
        {
            if (!EjectButton.IsEnabled)
                return;
            if (DllFileList.SelectedItem == null || _targetProcessId <= 0)
            {
                this.ShowMessageAsync("Needs additional input!", "Select a DLL and a target process before continuing.");
                return;
            }
            _currentInjector.EjectDll();
            _currentInjector.Dispose();
            InjectButton.IsEnabled = true;
            EjectButton.IsEnabled = false;
            this.ShowMessageAsync("Ejection successful!", "DLL has been ejected from process!");
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
            var dialog = new WnSelect { Owner = this };
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