using Libjector.Core;
using Libjector.Core.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Libjector.ViewModels;

namespace Libjector.Views;

public partial class SelectProcessWindow
{

    private SelectProcessWindowModel ViewModel => (SelectProcessWindowModel)DataContext;

    public KeyValuePair<int, string> SelectedProcess { get; private set; }

    public SelectProcessWindow()
    {
        InitializeComponent();
        ((CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource)).Filter = FilterProcesses;
    }

    private bool FilterProcesses(object item)
    {
        var filterText = FilterBox.Text;
        if (string.IsNullOrEmpty(filterText))
            return true; // does not filter item
        if (item is not ProcessItemBinding processItem)
            return false; // filter item
        return processItem.Id.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
               || processItem.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase);
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            if (process.MainWindowHandle == IntPtr.Zero)
                continue;
            ViewModel.ProcessList.Add(new ProcessItemBinding
            {
                Id = process.Id,
                Name = Path.GetFileName(process.MainModule.FileName ?? "Unidentified Process"),
                Architecture = Utilities.GetProcessArchitecture(process),
                Path = process.MainModule.FileName ?? string.Empty
            });
        }
    }

    private void OnProcessFilter(object sender, TextChangedEventArgs args)
    {
        CollectionViewSource.GetDefaultView(ProcessList.ItemsSource).Refresh();
    }

    private void OnProcessSelect(object sender, RoutedEventArgs args)
    {
        if (ProcessList.SelectedItem is not ProcessItemBinding item)
            return;
        ProcessBox.Text = $"{item.Name} ({item.Id})";
    }

    private void OnProcessSelected(object sender, MouseButtonEventArgs args)
    {
        if (ProcessList.SelectedItem is ProcessItemBinding)
            OnContinue(null, null);
    }

    private void OnContinue(object sender, RoutedEventArgs args)
    {
        if (ProcessList.SelectedItem is not ProcessItemBinding item)
            return;
        SelectedProcess = new KeyValuePair<int, string>(item.Id, item.Name);
        DialogResult = true;
        Close();
    }

}