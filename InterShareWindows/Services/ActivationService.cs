using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterShareWindows.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace InterShareWindows.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs);

        // Activate the MainWindow.
        App.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private Task InitializeAsync()
    {
        // await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
        return Task.CompletedTask;
    }

    private Task StartupAsync()
    {
        // await _themeSelectorService.SetRequestedThemeAsync();
        return Task.CompletedTask;
    }
}
