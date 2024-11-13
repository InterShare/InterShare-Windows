using CommunityToolkit.Mvvm.ComponentModel;
using InterShareSdk;

namespace InterShareWindows.Data;

public class DiscoveredDevice(SendProgress progress, Device device) : ObservableObject
{
    public SendProgress Progress { get; set; } = progress;
    public Device Device { get; set; } = device;
}
