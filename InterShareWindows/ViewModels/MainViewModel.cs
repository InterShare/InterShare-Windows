using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using ABI.System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WinUIEx;

namespace InterShareWindows.ViewModels;

public class MainViewModel : ViewModelBase
{
    public readonly AsyncRelayCommand SendFileCommand;
    public readonly AsyncRelayCommand SendClipCommand;
    public readonly RelayCommand OpenSettingsPageCommand;
    
    public MainViewModel(INavigationService navigationService) : base(navigationService)
    {
        SendFileCommand = new AsyncRelayCommand(SendFileAsync);
        SendClipCommand = new AsyncRelayCommand(SendClipAsync);
        OpenSettingsPageCommand = new RelayCommand(OpenSettingsPage);
    }

    private void OpenSettingsPage()
    {
        _navigationService.NavigateTo("InterShareWindows.ViewModels.SettingsViewModel");
    }

    private async Task SendFileAsync()
    {
        var picker = new FileOpenPicker
        {
            FileTypeFilter =
            {
                "*",
                ".jpg",
                ".jpeg",
                ".png",
                ".svg"
            }
        };
        
        var handler = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(picker, handler);
        
        var file = await picker.PickSingleFileAsync();
        var fileStream = await file.OpenStreamForReadAsync();
        
        await SendFileAsync(fileStream);
    }

    private Task SendClipAsync()
    {
        return SendFileAsync(null);
    }
    
    private async Task SendFileAsync(Stream fileStream)
    {
        _navigationService.NavigateTo("InterShareWindows.ViewModels.SelectRecipientViewModel");
        
        await Task.CompletedTask;
    }
    
}