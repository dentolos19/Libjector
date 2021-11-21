using Libjector.Core.Bindings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Libjector.ViewModels;

public class MainWindowModel : INotifyPropertyChanged
{

    private bool _isInjectionMode = true;
    private ObservableCollection<DllItemBinding> _dllList = new();

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set
        {
            _isInjectionMode = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DllItemBinding> DllList
    {
        get => _dllList;
        set
        {
            _dllList = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}