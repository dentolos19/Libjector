using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Libjector.Core.Bindings;

namespace Libjector.ViewModels;

public class SelectProcessWindowModel : INotifyPropertyChanged
{

    private ObservableCollection<ProcessItemBinding> _processList = new();

    public ObservableCollection<ProcessItemBinding> ProcessList
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