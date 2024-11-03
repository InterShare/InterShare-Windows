using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using ABI.System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Params;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WinUIEx;
using System.Linq;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using Windows.Devices.Radios;
using CommunityToolkit.Mvvm.ComponentModel;
using InterShareSdk;
using System.Threading;

namespace InterShareWindows.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly SynchronizationContext _uiContext;
    private NearbyService _nearbyServer;
    private Radio _bluetoothRadio;
    private readonly NavigationService _navigationService;
    public readonly AsyncRelayCommand SendFileCommand;
    public readonly AsyncRelayCommand SendClipCommand;
    public readonly RelayCommand OpenSettingsPageCommand;

    [ObservableProperty]
    private bool _bluetoothEnabled = true;

    public MainViewModel(NavigationService navigationService, NearbyService nearbyServer) : base()
    {
        _uiContext = SynchronizationContext.Current;
        _nearbyServer = nearbyServer;
        _navigationService = navigationService;
        SendFileCommand = new AsyncRelayCommand(SendFileAsync);
        SendClipCommand = new AsyncRelayCommand(SendClipboardAsync);
        OpenSettingsPageCommand = new RelayCommand(OpenSettingsPage);
        
        CheckBluetooth();
    }

    private async void CheckBluetooth()
    {
        var radios = await Radio.GetRadiosAsync();
        _bluetoothRadio = radios.FirstOrDefault(r => r.Kind == RadioKind.Bluetooth);

        if (_bluetoothRadio != null)
        {
            _bluetoothRadio.StateChanged += BluetoothRadio_StateChanged;
            CheckBluetoothStateAsync();
        }
    }

    private void BluetoothRadio_StateChanged(Radio sender, object args)
    {
        CheckBluetoothStateAsync();
    }

    private void CheckBluetoothStateAsync()
    {
        _uiContext.Post(_ =>
        {
            BluetoothEnabled = _bluetoothRadio.State == RadioState.On;

            if (BluetoothEnabled)
            {
                _nearbyServer.Start();
            }
            else
            {
                //_nearbyServer.NearbyServer.Stop();
            }
        }, null);
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
                "*"
            }
        };

        var handler = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(picker, handler);

        var files = await picker.PickMultipleFilesAsync();

        await SendFilesAsync(files.Select(file => file.Path).ToList());
    }

    private Task SendClipboardAsync()
    {
        return SendFilesAsync([]);
    }

    public async Task SendFilesAsync(List<string> filePaths)
    {
        if (filePaths.Count <= 0)
        {
            return;
        }

        var sendParameters = new SendParam
        {
            FilePaths = filePaths
        };

        _navigationService.NavigateTo("InterShareWindows.ViewModels.SelectRecipientViewModel", sendParameters);

        await Task.CompletedTask;
    }
}
