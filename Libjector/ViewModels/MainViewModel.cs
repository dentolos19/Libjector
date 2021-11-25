using Libjector.Models;
using System.Collections.ObjectModel;

namespace Libjector.ViewModels;

public class MainViewModel : BaseViewModel
{

    private bool _isInjectionMode = true;
    private ObservableCollection<DllItemModel> _dllList = new();

    public bool IsInjectionMode
    {
        get => _isInjectionMode;
        set => UpdateProperty(ref _isInjectionMode, value);
    }

    public ObservableCollection<DllItemModel> DllList
    {
        get => _dllList;
        set => UpdateProperty(ref _dllList, value);
    }

}