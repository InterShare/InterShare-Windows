using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;

namespace InterShareWindows.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(INavigationService navigationService) : base(navigationService)
    {
    }
}