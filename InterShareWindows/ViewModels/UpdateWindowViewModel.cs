using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShareWindows.ViewModels;

public partial class UpdateWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private int _progress;
}
