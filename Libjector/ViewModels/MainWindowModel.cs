using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Libjector.ViewModels;

public class MainWindowModel : INotifyPropertyChanged
{

    private bool _isInjectionMode = true;

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set
        {
            _isInjectionMode = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrEmpty(propertyName))
            return;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}