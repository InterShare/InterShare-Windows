using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using InterShareSdk;
using InterShareWindows.Data;
using InterShareWindows.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Params;
using System;
using static InterShareSdk.ConnectErrors;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace InterShareWindows.ViewModels;

public delegate void ShowDialogEvent();

public partial class SelectRecipientViewModel : ViewModelBase, IDiscoveryDelegate
{
    private readonly Discovery _discovery;
    private readonly NearbyService _nearbyService;
    private SynchronizationContext _uiContext;

    [ObservableProperty]
    private ObservableCollection<DiscoveredDevice> _devices;

    public SendParam SendParam { get; set; }

    public event ShowDialogEvent ShowBleNotAvailableDialog = delegate { };

    public SelectRecipientViewModel(NearbyService nearbyService) : base()
    {
        _nearbyService = nearbyService;
        _uiContext = SynchronizationContext.Current;
        Devices = [];
        _discovery = new Discovery(this);
        _discovery.Start();
    }

    public void Reset()
    {
        _discovery.Stop();
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

    [RelayCommand]
    public void Send(DiscoveredDevice device)
    {
        Task.Run(() =>
        {
            try
            {
                var files = SendParam.FilePaths;
                _nearbyService.NearbyServer.SendFiles(device.Device, files, device.Progress);
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
