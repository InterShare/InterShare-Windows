using CommunityToolkit.Mvvm.ComponentModel;

namespace InterShareWindows.ViewModels;

public partial class UpdateWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private int _progress;
}
