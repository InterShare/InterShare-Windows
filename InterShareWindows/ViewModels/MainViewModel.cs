using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Params;
using InterShareWindows.Services;
using WinRT.Interop;
using WinUIEx;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Windows.Devices.Radios;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading;
using Windows.ApplicationModel.DataTransfer;
using InterShareWindows.Data;
using InterShareWindows.Helper;

namespace InterShareWindows.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly SynchronizationContext _uiContext;
    private NearbyService? _nearbyServer;
    private Radio? _bluetoothRadio;
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private bool _bluetoothEnabled = true;
    
    [ObservableProperty]
    private bool _firewallBlocksInterShare;

    [ObservableProperty]
    private string _deviceName;

    public MainViewModel(NavigationService navigationService, NearbyService nearbyServer) : base()
    {
        _uiContext = SynchronizationContext.Current;
        _nearbyServer = nearbyServer;
        _navigationService = navigationService;
        DeviceName = LocalStorage.DeviceName;

        CheckBluetooth();
    }

    private async void CheckBluetooth()
    {
        try
        {
            var radios = await Radio.GetRadiosAsync();
            _bluetoothRadio = radios?.FirstOrDefault(r => r.Kind == RadioKind.Bluetooth);

            if (_bluetoothRadio != null)
            {
                _bluetoothRadio.StateChanged += BluetoothRadio_StateChanged;
                CheckBluetoothStateAsync();
            }
        
            FirewallBlocksInterShare = !FirewallChecker.IsProgramAllowedOnPrivateOrPublic();

            if (FirewallBlocksInterShare)
            {
                await FirewallChecker.StartBackgroundWatchUntilAllowed(
                    onAllowed: () =>
                    {
                        _uiContext.Post(_ => FirewallBlocksInterShare = false, null);
                    },
                    pollInterval: TimeSpan.FromSeconds(2)
                );
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
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
            BluetoothEnabled = _bluetoothRadio?.State == RadioState.On;

            if (BluetoothEnabled)
            {
                _nearbyServer?.Start();
            }
            else
            {
                //_nearbyServer.NearbyServer.Stop();
            }
        }, null);
    }

    [RelayCommand]
    private void OpenSettingsPage()
    {
        _navigationService.NavigateTo("InterShareWindows.ViewModels.SettingsViewModel");
    }

    [RelayCommand]
    private async Task SendFiles()
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

        SendFiles(files.Select(file => file.Path).ToList());
    }

    public void SendFiles(List<string> filePaths)
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
    }

    [RelayCommand]
    private async Task SendFolder()
    {
        var picker = new FolderPicker
        {
        };

        var handler = App.MainWindow.GetWindowHandle();
        InitializeWithWindow.Initialize(picker, handler);

        var folder = await picker.PickSingleFolderAsync();
        if (folder != null)
        {
            // Application now has read/write access to all contents in the picked folder
            // (including other sub-folder contents)
            // Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            SendFiles([folder.Path]);
        }
    }
    
    [RelayCommand]
    private async Task SendClipboard()
    {
        var package = Clipboard.GetContent();

        if (package.Contains(StandardDataFormats.StorageItems))
        {
            var storageItems = await package.GetStorageItemsAsync();
            var files = storageItems.Select(item => item.Path);

            SendFiles(files.ToList());
        }
        
        if (package.Contains(StandardDataFormats.Bitmap))
        {
            var bitmap = await package.GetBitmapAsync();

            if (bitmap != null)
            {
                using var storageItemStream = await bitmap.OpenReadAsync();
                var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".png");
                await using var fileStream = File.OpenWrite(tempFile);
                await storageItemStream.AsStreamForRead().CopyToAsync(fileStream);

                SendFiles([tempFile]);
            }
        }

        if (package.Contains(StandardDataFormats.Text))
        {
            var text = await package.GetTextAsync();

            if (!string.IsNullOrEmpty(text))
            {
                var sendParameters = new SendParam
                {
                    ClipboardContent = text
                };

                _navigationService.NavigateTo("InterShareWindows.ViewModels.SelectRecipientViewModel", sendParameters);   
            }
        }
    }
}
