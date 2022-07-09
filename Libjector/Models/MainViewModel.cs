using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Libjector.Models;

public partial class MainViewModel : ObservableObject
{

    [ObservableProperty] private bool _injectionMode = true;
    [ObservableProperty] private bool _hideDllFlag;
    [ObservableProperty] private bool _randomizeHeadersFlag;
    [ObservableProperty] private bool _randomizeNameFlag;
    [ObservableProperty] private ObservableCollection<DllItemModel> _dllList = new();

}