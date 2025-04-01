using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using InterShareSdk;
using InterShareWindows.Data;
using InterShareWindows.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Params;
using System;
using static InterShareSdk.ConnectErrors;
using System.Linq;

namespace InterShareWindows.ViewModels;

public delegate void ShowDialogEvent();

public partial class SelectRecipientViewModel : ViewModelBase, IDiscoveryDelegate
{
    private Discovery _discovery;
    private readonly NearbyService _nearbyService;
    private readonly SynchronizationContext _uiContext;
    private ShareStore? _shareStore;

    [ObservableProperty]
    private ObservableCollection<DiscoveredDevice> _devices;

    public ShareProgress ShareProgress { get; set; } = new(SynchronizationContext.Current!);

    public SendParam? SendParam { get; set; }

    public event ShowDialogEvent ShowBleNotAvailableDialog = delegate { };

    public SelectRecipientViewModel(NearbyService nearbyService) : base()
    {
        _nearbyService = nearbyService;
        _uiContext = SynchronizationContext.Current!;
        Devices = [];
        // ShareProgress.ProgressChanged(new ShareProgressState.Unknown());
        _discovery = new Discovery(this);
        _discovery.Start();
    }

    public void Reset()
    {
        _discovery.Stop();
        _discovery = new Discovery(this);
        Devices = [];
        _discovery.Start();
    }

    public void Stop()
    {
        _discovery.Stop();
    }

    public void DeviceAdded(Device value)
    {
        _uiContext.Post(_ => {
            if (Devices.FirstOrDefault(device => device.Device.id == value.id) == null)
            {
                Devices.Add(new DiscoveredDevice(new SendProgress(_uiContext), value));
            }
        }, null);
    }

    public void DeviceRemoved(string deviceId)
    {
        // Not implemented
    }

    public async Task Prepare()
    {
        ShareProgress.ProgressChanged(new ShareProgressState.Unknown());

        if (SendParam?.FilePaths != null)
        {
            _shareStore = await _nearbyService.NearbyServer.ShareFiles(SendParam.FilePaths, false, ShareProgress);
        }
        
        if (SendParam?.ClipboardContent != null)
        {
            _shareStore = await _nearbyService.NearbyServer.ShareText(SendParam.ClipboardContent, false);
            ShareProgress.ProgressChanged(new ShareProgressState.Finished());
        }
    }

    [RelayCommand]
    public void Send(DiscoveredDevice device)
    {
        Task.Run(async () =>
        {
            try
            {
                if (_shareStore == null)
                {
                    return;
                }

                await _shareStore.SendTo(device.Device, device.Progress);
            }
            catch (ConnectErrors error)
            {
                if (error is InternalBleHandlerNotAvailable)
                {
                    ShowBleNotAvailableDialog.Invoke();
                }

                _uiContext.Post(_ =>
                {
                    device.Progress.State = new SendProgressState.Unknown();
                }, null);

            }
            catch (Exception)
            {
                _uiContext.Post(_ =>
                {
                    device.Progress.State = new SendProgressState.Unknown();
                }, null);
            }
        });
    }
}
