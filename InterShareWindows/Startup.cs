using InterShareWindows.Activation;
using InterShareWindows.Services;
using InterShareWindows.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace InterShareWindows;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration = null)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        services.AddSingleton<ActivationService>();
        services.AddSingleton<PageService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<NearbyService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<SelectRecipientViewModel>();
    }
}