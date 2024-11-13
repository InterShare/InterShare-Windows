using InterShareWindows.Activation;
using InterShareWindows.Data;
using InterShareWindows.Services;
using InterShareWindows.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.IO.Pipes;

namespace InterShareWindows;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration = null)
    {
        _configuration = configuration;
    }

    private void RedirectConsoleOutput()
    {
        //try
        //{
        //    string isoDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        //    var logFilePath = Path.Combine(LocalStorage.SettingsFolderPath, $"InterShare-WinUI.log");
        //    var fileStream = new FileStream(logFilePath, FileMode.Create);
        //    var streamwriter = new StreamWriter(fileStream);
        //    streamwriter.AutoFlush = true;
        //    Console.SetOut(streamwriter);
        //    Console.SetError(streamwriter);
        //}
        //catch
        //{
        //    // Do nothing
        //}
    }

    public void ConfigureServices(IServiceCollection services)
    {
        RedirectConsoleOutput();
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        services.AddSingleton<ActivationService>();
        services.AddSingleton<PageService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<NearbyService>();
        services.AddSingleton<UpdateService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<SelectRecipientViewModel>();
        services.AddSingleton<UpdateWindowViewModel>();
    }
}