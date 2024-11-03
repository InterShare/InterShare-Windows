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
using Windows.Devices.Radios;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace InterShareWindows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainWindow : WindowEx
    {
        public Frame ContentSlot;
        private readonly NavigationService _navigationService;

        public MainWindow()
        {
            this.InitializeComponent();
            SystemBackdrop = new MicaBackdrop();
            this.SetWindowSize(480, 350);
            MinWidth = 400;
            MinHeight = 300;
            this.SetIsMaximizable(false);

            ExtendsContentIntoTitleBar = true;
            ContentSlot = ContentSlotEl as Frame;

            _navigationService = App.GetService<NavigationService>();
            _navigationService.Navigated += NavigationService_Navigated;
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            var canGoBack = _navigationService.CanGoBack;
            CustomTitleBar.IsBackButtonVisible = canGoBack;
            CustomTitleBar.IsBackEnabled = canGoBack;
        }

        private void OnBackRequested(TitleBar sender, object args)
        {
            _navigationService.GoBack();
        }
    }
}
