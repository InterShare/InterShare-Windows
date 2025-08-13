using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using InterShareWindows.Services;
using WinUIEx;
using Microsoft.UI.Composition.SystemBackdrops;
using TitleBar = WinUIEx.TitleBar;

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
            InitializeComponent();

            // if (DesktopAcrylicController.IsSupported())
            // {
            //     SystemBackdrop = new DesktopAcrylicBackdrop();
            // }
            // else
            // {
            //     SystemBackdrop = new MicaBackdrop();
            // }
            SystemBackdrop = new MicaBackdrop();

            this.SetWindowSize(600, 370);
            MinWidth = 600;
            MinHeight = 370;
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
