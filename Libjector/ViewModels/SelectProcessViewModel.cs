using Libjector.Models;
using System.Collections.ObjectModel;

namespace Libjector.ViewModels;

public class SelectProcessViewModel : BaseViewModel
{

    private ObservableCollection<ProcessItemModel> _processList = new();

    public ObservableCollection<ProcessItemModel> ProcessList
    {
        get => _processList;
        set => UpdateProperty(ref _processList, value);
    }

}