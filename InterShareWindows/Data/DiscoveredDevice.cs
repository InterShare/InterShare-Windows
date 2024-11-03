using CommunityToolkit.Mvvm.ComponentModel;
using InterShareSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;

namespace InterShareWindows.Data;

public partial class SendProgress(SynchronizationContext uiContext) : ObservableObject, SendProgressDelegate
{
    private SynchronizationContext _uiContext = uiContext;

    [ObservableProperty]
    private SendProgressState _state;

    public void ProgressChanged(SendProgressState progress)
    {
        _uiContext.Post(_ => State = progress, null);
    }
}

public class DiscoveredDevice(SendProgress progress, Device device) : ObservableObject
{
    public SendProgress Progress { get; set; } = progress;
    public Device Device { get; set; } = device;
}
