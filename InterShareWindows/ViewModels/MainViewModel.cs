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
    public readonly AsyncRelayCommand SendFileCommand;
    
    public MainViewModel(ITestService testService)
    {
        Name = testService.GetName();

        SendFileCommand = new AsyncRelayCommand(SendFileAsync);
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

    public string Name { get; set; }
}