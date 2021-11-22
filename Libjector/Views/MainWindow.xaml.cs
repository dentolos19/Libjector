using Bleak;
using Libjector.Core;
using Libjector.Core.Bindings;
using Libjector.ViewModels;
using Libjector.Views;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Libjector;

public partial class MainWindow
{

    private Process? _targetProcess;
    private Injector? _injectorService;

    private MainWindowModel ViewModel => (MainWindowModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        foreach (var libraryPath in App.Settings.SavedDllPaths)
        {
            ViewModel.DllList.Add(new DllItemBinding
            {
                Name = Path.GetFileName(libraryPath),
                Architecture = Utilities.GetDllArchitecture(libraryPath),
                Path = libraryPath
            });
        }
        MethodBox.SelectedIndex = App.Settings.SavedMethodIndex;
        HideDllOption.IsChecked = App.Settings.SavedHideDllFlagChecked;
        RandomizeHeaderOption.IsChecked = App.Settings.SavedRandomizeHeaderFlagChecked;
        RandomizeNameOption.IsChecked = App.Settings.SavedRandomizeNameFlagChecked;
    }

    private void OnSelectProcess(object sender, RoutedEventArgs args)
    {
        var dialog = new SelectProcessWindow { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        _targetProcess = dialog.SelectedProcess;
        ProcessBox.Text = $"{Path.GetFileName(_targetProcess.MainModule.FileName)} ({_targetProcess.Id})";
    }

    private void OnAddDlls(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Dynamic Link Library (*.dll)|*.dll", Multiselect = true };
        if (dialog.ShowDialog() != true)
            return;
        foreach (var filePath in dialog.FileNames)
        {
            var item = new DllItemBinding
            {
                Name = Path.GetFileName(filePath),
                Architecture = Utilities.GetDllArchitecture(filePath),
                Path = filePath
            };
            if (!ViewModel.DllList.Contains(item))
                ViewModel.DllList.Add(item);
        }
    }

    private void OnRemoveDll(object sender, RoutedEventArgs args)
    {
        if (DllList.SelectedItem is not DllItemBinding item)
            return;
        if (MessageBox.Show("Are you sure you want to remove this library?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        ViewModel.DllList.Remove(item);
    }

    private void OnRemoveAllDlls(object sender, RoutedEventArgs args)
    {
        if (!(DllList.Items.Count > 0))
            return;
        if (MessageBox.Show("Are you sure you want to remove all libraries?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        ViewModel.DllList.Clear();
    }

    private void OnInject(object sender, RoutedEventArgs args)
    {
        if (ViewModel.IsInjectionMode)
        {
            if (!Utilities.IsRunningAsAdministrator())
            {
                MessageBox.Show("Administrative privileges is required in order to inject a library into a process!", "Libjector");
                return;
            }
            if (_targetProcess is null)
            {
                MessageBox.Show("Select a target process before continuing!", "Libjector");
                return;
            }
            if (DllList.SelectedItem is not DllItemBinding libraryItem)
            {
                MessageBox.Show("Select a DLL before continuing!", "Libjector");
                return;
            }
            try
            {
                var flags = InjectionFlags.None;
                if (HideDllOption.IsChecked == true)
                    flags |= InjectionFlags.HideDllFromPeb;
                if (RandomizeHeaderOption.IsChecked == true)
                    flags |= InjectionFlags.RandomiseDllHeaders;
                if (RandomizeNameOption.IsChecked == true)
                    flags |= InjectionFlags.RandomiseDllName;
                var method = MethodBox.SelectedIndex switch
                {
                    1 => InjectionMethod.HijackThread,
                    2 => InjectionMethod.ManualMap,
                    _ => InjectionMethod.CreateThread
                };
                _injectorService?.Dispose();
                _injectorService = new Injector(_targetProcess.Id, libraryItem.Path, method, flags);
                _injectorService.InjectDll();
                if (flags.HasFlag(InjectionFlags.HideDllFromPeb))
                {
                    _injectorService.Dispose();
                }
                else
                {
                    InjectButton.Content = "Eject";
                    ViewModel.IsInjectionMode = false;
                }
                MessageBox.Show("The DLL has been injected into the process!", "Libjector");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while injecting: {exception.Message}", "Libjector");
            }
        }
        else
        {
            try
            {
                if (_injectorService is not null)
                {
                    _injectorService.EjectDll();
                    _injectorService.Dispose();
                }
                InjectButton.Content = "Inject";
                ViewModel.IsInjectionMode = true;
                MessageBox.Show("The DLL has been ejected from the process!", "Libjector");
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while ejecting: {exception.Message}", "Libjector");
            }
        }
    }

    private void OnClosing(object sender, CancelEventArgs args)
    {
        App.Settings.SavedDllPaths = ViewModel.DllList.Select(libraryItem => libraryItem.Path).ToArray();
        App.Settings.SavedMethodIndex = MethodBox.SelectedIndex;
        App.Settings.SavedHideDllFlagChecked = HideDllOption.IsChecked == true;
        App.Settings.SavedRandomizeHeaderFlagChecked = RandomizeHeaderOption.IsChecked == true;
        App.Settings.SavedRandomizeNameFlagChecked = RandomizeNameOption.IsChecked == true;
        App.Settings.Save();
    }

}