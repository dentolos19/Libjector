using System.Collections.ObjectModel;

namespace Libjector.Models;

public class MainViewModel : BaseViewModel
{

    private bool _isInjectionMode = true;

    public ObservableCollection<DllItemModel> DllList { get; } = new();

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set => UpdateProperty(ref _isInjectionMode, value);
    }

}