using System.Threading;
using System.Threading.Tasks;

namespace InterShareWindows.Helper;

using System;
using System.IO;

public static class FirewallChecker
{
    public static bool IsProgramAllowedOnPrivateOrPublic()
    {
        var processPath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

        if (string.IsNullOrEmpty(processPath))
        {
            return false;
        }
        
        var exePath = Path.GetFullPath(processPath);

        // ReSharper disable once SuspiciousTypeConversion.Global
        var policy2 = new NetFwPolicy2() as INetFwPolicy2;

        const int desiredProfiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE
                                    | (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC;

        if (policy2?.Rules is null)
        {
            return false;
        }
        
        foreach (INetFwRule r in policy2.Rules)
        {
            if (!r.Enabled) continue;
            if (r.Action != NET_FW_ACTION_.NET_FW_ACTION_ALLOW) continue;
            if (r.Direction != NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN) continue;

            // Program-based rule?
            if (!string.IsNullOrEmpty(r.ApplicationName)
                && string.Equals(r.ApplicationName, exePath, StringComparison.OrdinalIgnoreCase)
                && (r.Profiles & desiredProfiles) != 0)
            {
                return true;
            }
        }

        return false;
    }
    
    private static async Task WaitUntilAllowedAsync(TimeSpan? pollInterval = null, CancellationToken cancellationToken = default)
    {
        var interval = pollInterval ?? TimeSpan.FromSeconds(2);
        if (IsProgramAllowedOnPrivateOrPublic())
        {
            return;
        }

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (IsProgramAllowedOnPrivateOrPublic())
            {
                return;
            }

            try
            {
                await Task.Delay(interval, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }
    }

    /// <summary>
    /// Starts a background task that completes when allowed and then invokes onAllowed().
    /// Returns the Task so you can observe/log completion if you want.
    /// </summary>
    public static Task StartBackgroundWatchUntilAllowed(Action onAllowed, TimeSpan? pollInterval = null, CancellationToken cancellationToken = default)
    {
        if (onAllowed == null)
        {
            throw new ArgumentNullException(nameof(onAllowed));
        }

        return Task.Run(async () =>
        {
            await WaitUntilAllowedAsync(pollInterval, cancellationToken).ConfigureAwait(false);
            if (!cancellationToken.IsCancellationRequested)
            {
                onAllowed();
            }
        }, cancellationToken);
    }
}
