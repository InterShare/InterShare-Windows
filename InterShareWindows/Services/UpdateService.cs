using InterShareWindows.ViewModels;
using InterShareWindows.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Velopack;
using WinUIEx;

namespace InterShareWindows.Services;

public class UpdateService(UpdateWindowViewModel viewModel)
{
    public async Task<bool> Update()
    {
        try
        {
            var uiContext = SynchronizationContext.Current;
            var manager = new UpdateManager("https://intershare.app/windows-download");

            var newVersion = await manager.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return false;
            }

            var updateWindow = new UpdateWindow();
            updateWindow.Show();

            viewModel.Version = newVersion.TargetFullRelease?.Version?.ToFullString() ?? "Unknown Version";

            await manager.DownloadUpdatesAsync(newVersion, progress =>
            {
                uiContext.Post(_ => viewModel.Progress = progress, null);
            });

            updateWindow.Close();

            manager.ApplyUpdatesAndRestart(newVersion);

            return true;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
        }

        return false;
    }
}
