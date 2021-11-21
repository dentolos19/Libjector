using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Libjector.Core.Bindings;

namespace Libjector.ViewModels;

public class MainWindowModel : INotifyPropertyChanged
{

    private bool _isInjectionMode = true;
    private ObservableCollection<LibraryItemBinding> _libraryList = new();

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set
        {
            _isInjectionMode = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<LibraryItemBinding> LibraryList
    {
        get => _libraryList;
        set
        {
            _libraryList = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}