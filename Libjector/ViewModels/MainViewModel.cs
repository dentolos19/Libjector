using Libjector.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Libjector.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{

    private bool _isInjectionMode = true;
    private ObservableCollection<DllItemModel> _dllList = new();

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set
        {
            _isInjectionMode = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<DllItemModel> DllList
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