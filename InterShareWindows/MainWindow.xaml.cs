using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using InterShareSdk;
using InterShareWindows.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace InterShareWindows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx, NearbyConnectionDelegate
    {
        public Frame ContentSlot;
        private readonly INavigationService _navigationService;

        public MainWindow()
        {
            // var manager = WindowManager.Get(this);
            this.InitializeComponent();
            SystemBackdrop = new MicaBackdrop();
            this.SetWindowSize(480, 350);
            ExtendsContentIntoTitleBar = true;
            ContentSlot = ContentSlotEl as Frame;

            _navigationService = App.GetService<INavigationService>();
            _navigationService.Navigated += NavigationService_Navigated;

            // SetTitleBar(TitleBar);
            // ExtendsContentIntoTitleBar = true;
            // AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            // var hwnd = WindowNative.GetWindowHandle(this);
            // WindowId id = Win32Interop.GetWindowIdFromWindow(hwnd);
            // var appWindow = AppWindow.GetFromWindowId(id);
            // if (true)
            // {
            //     var titleBar = appWindow.TitleBar;
            //     titleBar.ExtendsContentIntoTitleBar = true;
            //     titleBar.ButtonBackgroundColor = Colors.Transparent;
            //     titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            // }

            // var device = new Device(id: "d5a5eab4-4dc6-46ae-991c-2c7ada359ac8", name: "Windows PC", deviceType: 0);
            // var nearbyServer = new NearbyServer(device, this);
            // nearbyServer.Start();
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            var canGoBack = _navigationService.CanGoBack;
            CustomTitleBar.IsBackEnabled = canGoBack;
        }

        public void ReceivedConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine("Received connection request");
        }

        private void OnBackRequested(TitleBar sender, object args)
        {
            _navigationService.GoBack();
        }
    }
}
