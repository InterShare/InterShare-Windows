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

        public MainWindow()
        {
            var manager = WindowManager.Get(this);

            SystemBackdrop = new MicaBackdrop();
            this.SetWindowSize(480, 350);

            this.InitializeComponent();

            ContentSlot = ContentSlotEl as Frame;

            SetTitleBar(AppTitleBar);
            ExtendsContentIntoTitleBar = true;

            var device = new Device(id: "d5a5eab4-4dc6-46ae-991c-2c7ada359ac8", name: "Windows PC", deviceType: 0);
            var nearbyServer = new NearbyServer(device, this);
            nearbyServer.Start();
        }

        public void ReceivedConnectionRequest(ConnectionRequest request)
        {
            Console.WriteLine("Received connection request");
        }
    }
}
