using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InterShareWindows.Services;

public class NearbyService : NearbyConnectionDelegate, ReceiveProgressDelegate
{
    private ConnectionRequest? _currentConnectionRequest;
    private ReceiveProgressState? _currentProgress;
    private bool _startedNotificationLoop = false;
    private NearbyServer _nearbyServer;

    public NearbyServer NearbyServer {
        get
        {
            if (_nearbyServer == null)
            {
                var device = new Device(name: LocalStorage.DeviceName, id: LocalStorage.DeviceId.ToString(), deviceType: 0);
                _nearbyServer = new NearbyServer(device, this);
            }

            return _nearbyServer;
        }
    }
    private uint _sequenceNumber = 1;
    private string _notificationTag;
    private uint _notificationId;

    public NearbyService()
    {
    }

    public void Start()
    {
        NearbyServer.Start();
    }

    public void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
        AppNotificationManager.Default.Register();
    }

    public void SendUpdatableToastWithProgress()
    {
        var sender = _currentConnectionRequest.GetSender().name ?? "Unknown";
        var fileTransferIntent = _currentConnectionRequest.GetFileTransferIntent();

        var files = fileTransferIntent.fileCount > 1 ? $"{fileTransferIntent.fileCount} files" : $"{fileTransferIntent.fileName}";
        var text = $"{sender} wants to send you {files}";

        var builder = new AppNotificationBuilder()
            .AddText(text)
            .AddButton(new AppNotificationButton("Accept")
                .AddArgument("action", "accept"))
            .AddButton(new AppNotificationButton("Decline")
                .AddArgument("action", "decline"))
            .SetDuration(AppNotificationDuration.Long)
            .SetScenario(AppNotificationScenario.Urgent);

        var notification = builder.BuildNotification();

        AppNotificationManager.Default.Show(notification);
        _notificationId = notification.Id;
    }

    private async void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
    {
        await sender.RemoveByIdAsync(_notificationId);
        await Task.Delay(500);

        if (args.Arguments.ContainsKey("action"))
        {
            var action = args.Arguments["action"];

            if (action == "accept")
            {
                _currentConnectionRequest.SetProgressDelegate(this);
                _sequenceNumber = 1;

                var _ = Task.Run(() =>
                {
                    _currentConnectionRequest.Accept();
                });

                _notificationTag = "NotificationProgress";

                var senderName = _currentConnectionRequest.GetSender().name ?? "Unknown";
                var fileTransferIntent = _currentConnectionRequest.GetFileTransferIntent();

                var files = fileTransferIntent.fileCount > 1 ? $"{fileTransferIntent.fileCount} files" : $"{fileTransferIntent.fileName}";
                var text = $"Receiving {files} from {senderName}";

                var progressBuilder = new AppNotificationBuilder()
                    .AddText(text)
                    .AddProgressBar(new AppNotificationProgressBar()
                            .BindStatus()
                            .BindValue()
                            .BindValueStringOverride()
                        )
                        .SetTag(_notificationTag)

                    .AddButton(new AppNotificationButton("Cancel")
                        .AddArgument("action", "cancel"))
                    .SetScenario(AppNotificationScenario.Urgent);

                sender.Show(progressBuilder.BuildNotification());
            }
            else if (action == "decline")
            {
                _currentConnectionRequest.Decline();
            }
            else if (action == "cancel")
            {
                _currentConnectionRequest.Cancel();
            }
            else if (action == "open-downloads")
            {

                try
                {
                    string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    Process.Start("explorer.exe", downloadsPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to open Downloads folder: {ex.Message}");
                }
            }
        }
        else
        {
            _currentConnectionRequest?.Decline();
            _currentConnectionRequest = null;
        }
    }

    public void ReceivedConnectionRequest(ConnectionRequest request)
    {
        if (_currentConnectionRequest != null)
        {
            request.Decline();
            return;
        }

        _currentConnectionRequest = request;
        SendUpdatableToastWithProgress();
    }

    private void UpdateNotificationLoop()
    {
        if (_startedNotificationLoop)
        {
            return;
        }

        _startedNotificationLoop = true;

        Task.Run(async () =>
        {
            var currentSequenceNumber = 0u;
            while (true)
            {
                if (_sequenceNumber <= currentSequenceNumber)
                {
                    continue;
                }
                currentSequenceNumber = _sequenceNumber;

                var progress = _currentProgress;

                Console.WriteLine(_sequenceNumber);
                Console.WriteLine(progress);

                if (progress is ReceiveProgressState.Receiving receivingProgress)
                {
                    await AppNotificationManager.Default.UpdateAsync(new AppNotificationProgressData(1)
                    {
                        SequenceNumber = _sequenceNumber,
                        Value = receivingProgress.progress,
                        ValueStringOverride = receivingProgress.progress.ToString("P", CultureInfo.InvariantCulture),
                        Status = "Receiving"
                    }, _notificationTag);
                }
                if (progress is ReceiveProgressState.Extracting)
                {
                    await AppNotificationManager.Default.UpdateAsync(new AppNotificationProgressData(1)
                    {
                        SequenceNumber = _sequenceNumber,
                        Value = 1.0,
                        ValueStringOverride = "100.00 %",
                        Status = "Extracting"
                    }, _notificationTag);
                }
                if (progress is ReceiveProgressState.Finished)
                {
                    await AppNotificationManager.Default.RemoveAllAsync();

                    var builder = new AppNotificationBuilder()
                        .AddText("Finished Transfer")
                        .AddButton(new AppNotificationButton("Open Downloads")
                            .AddArgument("action", "open-downloads"))
                        .SetDuration(AppNotificationDuration.Long)
                        .SetScenario(AppNotificationScenario.Urgent);

                    var notification = builder.BuildNotification();

                    AppNotificationManager.Default.Show(notification);

                    _startedNotificationLoop = false;
                    _currentConnectionRequest = null;
                    return;
                }
                if (progress is ReceiveProgressState.Cancelled)
                {
                    await AppNotificationManager.Default.RemoveAllAsync();

                    var builder = new AppNotificationBuilder()
                        .AddText("Transfer cancelled")
                        .SetDuration(AppNotificationDuration.Long)
                        .SetScenario(AppNotificationScenario.Urgent);

                    var notification = builder.BuildNotification();

                    AppNotificationManager.Default.Show(notification);

                    _startedNotificationLoop = false;
                    _currentConnectionRequest = null;
                    return;
                }
            }
        });
    }

    public void ProgressChanged(ReceiveProgressState progress)
    {
        _currentProgress = progress;
        _sequenceNumber += 1;

        UpdateNotificationLoop();
        
        //Task.Run(async () =>
        //{
        //});
    }
}
