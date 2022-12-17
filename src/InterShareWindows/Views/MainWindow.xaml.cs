// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace InterShareWindows.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            var manager = WindowManager.Get(this);
            manager.Backdrop = new MicaSystemBackdrop();
            this.SetWindowSize(380, 250);

            this.InitializeComponent();

            SetTitleBar(AppTitleBar);
            ExtendsContentIntoTitleBar = true;
        }

        private void OnDragOver(object sender, DragEventArgs eventArgs)
        {
            eventArgs.AcceptedOperation = DataPackageOperation.Copy;
            DropZone.Background = new SolidColorBrush(Color.FromArgb(200, 80, 91, 250));
        }

        private void OnDragLeave(object sender, DragEventArgs eventArgs)
        {
            eventArgs.AcceptedOperation = DataPackageOperation.Copy;
            DropZone.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void OnDrop(object sender, DragEventArgs eventArgs)
        {
            eventArgs.AcceptedOperation = DataPackageOperation.Copy;
            DropZone.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
