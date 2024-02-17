using System;
using System.Diagnostics;
using System.Net.Mime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using ABI.System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

namespace InterShareWindows.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    public readonly AsyncRelayCommand SendFileCommand;
    public readonly AsyncRelayCommand SendClipCommand;
    public readonly RelayCommand OpenSettingsPageCommand;
    
    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
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
        var picker = new FileOpenPicker();
        
        var handler = App.MainWindow.GetWindowHandle();
        WinRT.Interop.InitializeWithWindow.Initialize(picker, handler);
        
        picker.FileTypeFilter.Add("*");
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");
        picker.FileTypeFilter.Add(".svg");
        
        var file = await picker.PickSingleFileAsync();
    }

    private async Task SendClipAsync()
    {
        DataPackageView clipContent = Clipboard.GetContent();

        if (clipContent.Contains(StandardDataFormats.Text))
        {
            string text = await clipContent.GetTextAsync();
        }
        else
        {
            //Todo: If clipContent is is a file, the file should be handled as selected via sendFile
        }
    }
}