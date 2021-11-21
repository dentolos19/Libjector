using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Bleak;
using Libjector.Core;
using Libjector.Core.Bindings;
using Libjector.ViewModels;
using Libjector.Views;
using Microsoft.Win32;

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

    private void UpdateLibraryPaths()
    {
        App.Settings.LibraryPaths = ViewModel.LibraryList.Select(libraryItem => libraryItem.Path).ToArray();
        App.Settings.Save();
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        foreach (var libraryPath in App.Settings.LibraryPaths)
        {
            ViewModel.LibraryList.Add(new LibraryItemBinding
            {
                Name = Path.GetFileName(libraryPath),
                Architecture = Utilities.GetLibraryArchitecture(libraryPath),
                Path = libraryPath
            });
        }
    }

    private void OnSelectProcess(object sender, RoutedEventArgs args)
    {
        var dialog = new SelectProcessDialog { Owner = this };
        if (dialog.ShowDialog() != true)
            return;
        _targetProcess = dialog.SelectedProcess;
        ProcessBox.Text = $"{Path.GetFileName(_targetProcess.MainModule.FileName)} ({_targetProcess.Id})";
    }

    private void OnAddLibraries(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog { Filter = "Dynamic Link Library (*.dll)|*.dll", Multiselect = true };
        if (dialog.ShowDialog() != true)
            return;
        foreach (var filePath in dialog.FileNames)
        {
            var item = new LibraryItemBinding
            {
                Name = Path.GetFileName(filePath),
                Architecture = Utilities.GetLibraryArchitecture(filePath),
                Path = filePath
            };
            if (!ViewModel.LibraryList.Contains(item))
                ViewModel.LibraryList.Add(item);
        }
        UpdateLibraryPaths();
    }

    private void OnRemoveLibrary(object sender, RoutedEventArgs args)
    {
        if (LibraryList.SelectedItem is not LibraryItemBinding item)
            return;
        if (MessageBox.Show("Are you sure you want to remove this library?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        ViewModel.LibraryList.Remove(item);
        UpdateLibraryPaths();
    }

    private void OnClearLibraries(object sender, RoutedEventArgs args)
    {
        if (!(LibraryList.Items.Count > 0))
            return;
        if (MessageBox.Show("Are you sure you want to clear all libraries?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        ViewModel.LibraryList.Clear();
        UpdateLibraryPaths();
    }

    private void OnInject(object sender, RoutedEventArgs args)
    {
        if (ViewModel.IsInjectionMode)
        {
            if (!Utilities.IsRunningAsAdministrator())
            {
                MessageBox.Show("Administrative privileges is required in order to inject a library into a process.", "Libjector");
                return;
            }
            if (_targetProcess is null)
            {
                MessageBox.Show("Select a target process before continuing.", "Libjector");
                return;
            }
            if (LibraryList.SelectedItem is not LibraryItemBinding libraryItem)
            {
                MessageBox.Show("Select a library before continuing.", "Libjector");
                return;
            }
            try
            {
                _injectorService?.Dispose();
                _injectorService = new Injector(_targetProcess.Id, libraryItem.Path, InjectionMethod.CreateThread);
                _injectorService.InjectDll();
                InjectButton.Content = "Eject";
                ViewModel.IsInjectionMode = false;
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
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while ejecting: {exception.Message}", "Libjector");
            }
        }
    }

}