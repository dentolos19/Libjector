using System;
using System.Diagnostics;
using System.IO;
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

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        // TODO: load previously added libraries
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
            if (!LibraryList.Items.Contains(item))
                LibraryList.Items.Add(item);
        }
    }

    private void OnRemoveLibrary(object sender, RoutedEventArgs args)
    {
        if (LibraryList.SelectedItem is not LibraryItemBinding item)
            return;
        if (MessageBox.Show("Are you sure you want to remove this library?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        LibraryList.Items.Remove(item);
    }

    private void OnClearLibraries(object sender, RoutedEventArgs args)
    {
        if (!(LibraryList.Items.Count > 0))
            return;
        if (MessageBox.Show("Are you sure you want to clear all libraries?", "Libjector", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            return;
        LibraryList.Items.Clear();
    }

    private void OnInject(object sender, RoutedEventArgs args)
    {
        var viewModel = (MainWindowModel)DataContext;
        if (viewModel.IsInjectionMode)
        {
            if (_targetProcess is null)
            {
                MessageBox.Show("Please select a target process.", "Libjector");
                return;
            }
            if (LibraryList.SelectedItem is not LibraryItemBinding libraryItem)
            {
                MessageBox.Show("Please select a library.", "Libjector");
                return;
            }
            try
            {
                _injectorService?.Dispose();
                _injectorService = new Injector(_targetProcess.Id, libraryItem.Path, InjectionMethod.CreateThread);
                _injectorService.InjectDll();
                InjectButton.Content = "Eject";
                viewModel.IsInjectionMode = false;
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
                viewModel.IsInjectionMode = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while ejecting: {exception.Message}", "Libjector");
            }
        }
    }

}