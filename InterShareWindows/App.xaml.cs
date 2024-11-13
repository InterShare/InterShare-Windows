using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ABI.Windows.Security.Authentication.Identity.Core;
using InterShareWindows.Activation;
using InterShareWindows.Services;
using InterShareWindows.ViewModels;
using Microsoft.Extensions.Configuration;
using WinUIEx;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;
using Microsoft.Windows.AppNotifications;
using Velopack;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace InterShareWindows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public static MainWindow MainWindow { get; } = new MainWindow();
        
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            VelopackApp.Build().Run();
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            /*var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            var configuration = builder.Build();*/

            _host = Host.CreateDefaultBuilder().UseContentRoot(AppContext.BaseDirectory).ConfigureServices(services =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services);
                }
            ).Build();
        }
        
        public static T GetService<T>()
            where T : class
        {
            if ((App.Current as App)!._host.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }

            return service;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var nearbyService = GetService<NearbyService>();
            nearbyService.Initialize();

            await GetService<ActivationService>().ActivateAsync(args);
            await GetService<UpdateService>().Update();
        }

        void OnProcessExit(object sender, EventArgs e)
        {
            AppNotificationManager.Default.Unregister();
        }
    }
}
