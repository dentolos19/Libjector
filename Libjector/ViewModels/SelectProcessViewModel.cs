using Libjector.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Libjector.ViewModels;

public class SelectProcessViewModel : INotifyPropertyChanged
{

    private ObservableCollection<ProcessItemModel> _processList = new();

    public ObservableCollection<ProcessItemModel> ProcessList
    {
        get => _processList;
        set
        {
            _processList = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}