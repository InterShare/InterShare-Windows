using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;

namespace InterShareWindows.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    public readonly RelayCommand GoBackCommand;

    public SettingsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        GoBackCommand = new RelayCommand(GoBack);
    }

    private void GoBack()
    {
        _navigationService.GoBack();
    }
}