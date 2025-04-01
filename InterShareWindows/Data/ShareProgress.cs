using System;
using CommunityToolkit.Mvvm.ComponentModel;
using InterShareSdk;
using System.Threading;
using System.Timers;

namespace InterShareWindows.Data;

public partial class ShareProgress : ObservableObject, ShareProgressDelegate
{
    private SynchronizationContext _uiContext;
    private ShareProgressState _latestState;
    private readonly System.Timers.Timer _updateTimer;

    [ObservableProperty]
    private ShareProgressState _state = new ShareProgressState.Unknown();

    public ShareProgress(SynchronizationContext uiContext)
    {
        _uiContext = uiContext;

        _updateTimer = new System.Timers.Timer(200); // Throttle updates every 200 ms
        _updateTimer.Elapsed += UpdateProgress;
        _updateTimer.AutoReset = false;
    }

    private void UpdateProgress(object? sender, ElapsedEventArgs e)
    {
        _uiContext.Post(_ =>
        {
            // Update the UI with the latest state
            State = _latestState;
            Console.WriteLine($"State: {State}");
        }, null);
    }

    public void ProgressChanged(ShareProgressState progress)
    {
        _latestState = progress;
        if (!_updateTimer.Enabled)
        {
            _updateTimer.Start();
        }
    }
}
