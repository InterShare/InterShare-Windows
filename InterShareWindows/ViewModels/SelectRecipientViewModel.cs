using System.Collections.ObjectModel;
using System.Threading;
using Windows.System;
using InterShareSdk;
using InterShareWindows.Params;
using InterShareWindows.Services;

namespace InterShareWindows.ViewModels;

public class SelectRecipientViewModel : ViewModelBase, DiscoveryDelegate
{
    private readonly Discovery _discovery;
    private ObservableCollection<Device> _devices;
    private readonly SynchronizationContext _uiContext;

    public SendParam SendParam { get; set; }

    public ObservableCollection<Device> Devices
    {
        get => _devices;
        set => SetProperty(ref _devices, value);
    }

    public SelectRecipientViewModel(INavigationService navigationService) : base(navigationService)
    {
        _uiContext = SynchronizationContext.Current;
        Devices = [];
        _discovery = new Discovery(this);
        _discovery.Start();
    }

    public void DeviceAdded(Device value)
    {
        _uiContext.Post(_ => Devices.Add(value), null);
    }

    public void DeviceRemoved(string deviceId)
    {
        // Not implemented
    }
}
