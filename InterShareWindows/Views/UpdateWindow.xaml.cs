using InterShareWindows.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Windowing;
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
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace InterShareWindows.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private AppWindow _apw;
        private OverlappedPresenter _presenter;
        protected UpdateWindowViewModel ViewModel { get; }

        public UpdateWindow()
        {
            this.InitializeComponent();

            ViewModel = App.GetService<UpdateWindowViewModel>();
            RootGrid.DataContext = ViewModel;

            ExtendsContentIntoTitleBar = true;
            SystemBackdrop = new MicaBackdrop();
            this.SetWindowSize(400, 120);
            this.SetIsMaximizable(false);
            this.SetIsResizable(false);
            this.SetIsMinimizable(false);
            GetAppWindowAndPresenter();
            _presenter.SetBorderAndTitleBar(true, false);

            SetTitleBar(ContentFrame);
        }

        public void GetAppWindowAndPresenter()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            _apw = AppWindow.GetFromWindowId(myWndId);
            _presenter = _apw.Presenter as OverlappedPresenter;
        }
    }
}
