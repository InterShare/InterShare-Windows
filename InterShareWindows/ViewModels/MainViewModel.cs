using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using ABI.System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace InterShareWindows.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    public readonly AsyncRelayCommand SendFileCommand;
    public readonly RelayCommand OpenSettingsPageCommand;
    
    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        SendFileCommand = new AsyncRelayCommand(SendFileAsync);
        OpenSettingsPageCommand = new RelayCommand(OpenSettingsPage);
    }

    private void OpenSettingsPage()
    {
        _navigationService.NavigateTo("InterShareWindows.ViewModels.SettingsViewModel");
    }

    private async Task SendFileAsync()
    {
        var picker = new FileOpenPicker();
        
        var hwnd = App.MainWindow.GetWindowHandle();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        
        picker.FileTypeFilter.Add("*");
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");
        picker.FileTypeFilter.Add(".svg");
        
        var file = await picker.PickSingleFileAsync();
    }
}