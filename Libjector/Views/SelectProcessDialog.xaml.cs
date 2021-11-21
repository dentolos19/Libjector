using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Libjector.Core;
using Libjector.Core.Bindings;

namespace Libjector.Views;

public partial class SelectProcessDialog
{

    public Process SelectedProcess { get; private set; }

    public SelectProcessDialog()
    {
        InitializeComponent();
    }

    private void OnInitialized(object sender, EventArgs args)
    {
        var processes = Process.GetProcesses();
        foreach (var process in processes)
        {
            if (string.IsNullOrEmpty(process.MainWindowTitle))
                continue;
            ProcessList.Items.Add(new ProcessItemBinding
            {
                Id = process.Id,
                Name = Path.GetFileName(process.MainModule.FileName ?? "Unidentified Process"),
                Architecture = Utilities.GetProcessArchitecture(process),
                Path = process.MainModule.FileName ?? string.Empty,
                Process = process
            });
        }
    }

    private void OnProcessSelect(object sender, RoutedEventArgs args)
    {
        if (ProcessList.SelectedItem is not ProcessItemBinding item)
            return;
        ProcessBox.Text = $"{Path.GetFileName(item.Process.MainModule.FileName)} ({item.Process.Id})";
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
        SelectedProcess = item.Process;
        DialogResult = true;
        Close();
    }

}