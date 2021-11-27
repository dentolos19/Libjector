using Libjector.Core;
using Libjector.Models;
using Libjector.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Libjector.Views;

public partial class SelectProcessWindow
{

    private SelectProcessViewModel ViewModel => (SelectProcessViewModel)DataContext;

    public KeyValuePair<int, string> SelectedProcess { get; private set; }

    public SelectProcessWindow()
    {
        InitializeComponent();
        ((CollectionView)CollectionViewSource.GetDefaultView(ProcessList.ItemsSource)).Filter = FilterProcesses;
    }

    private bool FilterProcesses(object item)
    {
        var filterText = FilterInput.Text;
        if (string.IsNullOrEmpty(filterText))
            return true; // does not filter item (keeps it)
        if (item is not ProcessItemModel processItem)
            return false; // filter item (hides it)
        return processItem.Id.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase) // checks process id
               || processItem.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase); // checks process name
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            if (process.MainWindowHandle == IntPtr.Zero)
                continue; // continues the loop; if the process doesn't have a window or it is a background process
            ViewModel.ProcessList.Add(new ProcessItemModel(process.Id, Path.GetFileName(process.MainModule?.FileName ?? "Unidentified Process"), Utilities.GetProcessArchitecture(process), process.MainModule?.FileName ?? string.Empty));
        }
    }

    private void OnProcessFilter(object sender, TextChangedEventArgs args)
    {
        CollectionViewSource.GetDefaultView(ProcessList.ItemsSource).Refresh();
    }

    private void OnProcessSelect(object sender, RoutedEventArgs args)
    {
        if (ProcessList.SelectedItem is not ProcessItemModel item)
            return;
        ProcessInput.Text = $"{item.Name} ({item.Id})";
    }

    private void OnProcessSelected(object sender, MouseButtonEventArgs args)
    {
        if (ProcessList.SelectedItem is ProcessItemModel)
            OnContinue(null, null);
    }

    private void OnContinue(object sender, RoutedEventArgs args)
    {
        if (ProcessList.SelectedItem is not ProcessItemModel item)
            return;
        SelectedProcess = new KeyValuePair<int, string>(item.Id, item.Name);
        DialogResult = true;
        Close();
    }

}