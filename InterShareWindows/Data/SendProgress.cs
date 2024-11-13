using CommunityToolkit.Mvvm.ComponentModel;
using InterShareSdk;
using System;
using System.Threading;
using System.Timers;

namespace InterShareWindows.Data;

public partial class SendProgress : ObservableObject, SendProgressDelegate
{
    private SynchronizationContext _uiContext;
    private SendProgressState _latestState;
    private readonly System.Timers.Timer _updateTimer;

    [ObservableProperty]
    private SendProgressState _state;

    public SendProgress(SynchronizationContext uiContext)
    {
        _uiContext = uiContext;

        _updateTimer = new System.Timers.Timer(200); // Throttle updates every 200 ms
        _updateTimer.Elapsed += UpdateProgress;
        _updateTimer.AutoReset = false;
    }

    private void UpdateProgress(object sender, ElapsedEventArgs e)
    {
        _uiContext.Post(_ =>
        {
            // Update the UI with the latest state
            State = _latestState;
        }, null);
    }

    public void ProgressChanged(SendProgressState progress)
    {
        _latestState = progress;
        if (!_updateTimer.Enabled)
        {
            _updateTimer.Start();
        }
    }
}
