using System.Collections.ObjectModel;
using InterShareWindows.Services;
using InterShareWindows.Views;

namespace InterShareWindows.ViewModels;

public class SelectRecipientViewModel : ViewModelBase
{
    private ObservableCollection<Device> _devices;

    public ObservableCollection<Device> Devices
    {
        get => _devices;
        set => SetProperty(ref _devices, value);
    }
    
    public SelectRecipientViewModel(INavigationService navigationService) : base(navigationService)
    {
        Devices = new ObservableCollection<Device>
        {
            new Device { Name = "Device 1", DeviceType = "Mobile"},
            new Device { Name = "Device 2", DeviceType = "PC"},
            new Device { Name = "Device 3", DeviceType = "Mobile"}
        };
    }
}