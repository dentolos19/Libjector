using Bleak;
using Libjector.Core;
using Libjector.Models;
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

    private int? _targetProcessId;
    private Injector? _injectorService;
    private BackgroundWorker _processHandler;

    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void ToggleInjectionMode(bool state)
    {
        InjectButton.Content = state ? "Inject" : "Eject";
        ViewModel.IsInjectionMode = state;
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        foreach (var libraryPath in App.Settings.SavedDllPaths)
        {
            ViewModel.DllList.Add(new DllItemModel
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
        _targetProcessId = dialog.SelectedProcess.Key;
        ProcessBox.Text = $"{dialog.SelectedProcess.Value} ({_targetProcessId})";
    }

    private void OnAddDlls(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Dynamic Link Library (*.dll)|*.dll", Multiselect = true };
        if (dialog.ShowDialog() != true)
            return;
        foreach (var filePath in dialog.FileNames)
        {
            var item = new DllItemModel
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
        if (DllList.SelectedItem is not DllItemModel item)
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
            if (_targetProcessId is null)
            {
                MessageBox.Show("Select a target process before continuing!", "Libjector");
                return;
            }
            if (DllList.SelectedItem is not DllItemModel dllItem)
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
                _injectorService = new Injector(_targetProcessId.Value, dllItem.Path, method, flags);
                _injectorService.InjectDll();
                if (flags.HasFlag(InjectionFlags.HideDllFromPeb))
                {
                    _injectorService.Dispose();
                }
                else
                {
                    _processHandler?.Dispose();
                    _processHandler = new BackgroundWorker { WorkerSupportsCancellation = true };
                    _processHandler.DoWork += delegate
                    {
                        try
                        {
                            using var process = Process.GetProcessById(_targetProcessId.Value);
                            process.WaitForExit();
                            Debug.WriteLine("The target process has ended.");
                        }
                        catch
                        {
                            // do nothing
                        }
                    };
                    _processHandler.RunWorkerCompleted += delegate
                    {
                        _injectorService?.Dispose();
                        ToggleInjectionMode(true);
                    };
                    _processHandler.RunWorkerAsync();
                    ToggleInjectionMode(false);
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
                _injectorService?.EjectDll();
                _injectorService?.Dispose();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while ejecting: {exception.Message}", "Libjector");
            }
            if (_processHandler?.IsBusy == true)
                _processHandler?.CancelAsync();
            _processHandler?.Dispose();
            ToggleInjectionMode(true);
            MessageBox.Show("The DLL has been ejected from the process!", "Libjector");
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