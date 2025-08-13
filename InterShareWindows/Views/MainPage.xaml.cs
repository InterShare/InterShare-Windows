using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using InterShareWindows.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using Windows.Storage;
using System;
using InterShareWindows.Data;
using InterShareWindows.Helper;

namespace InterShareWindows.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        DataContext = ViewModel;
        InitializeComponent();

        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!LocalStorage.DidAlreadyShowBluetoothNote)
        {
            LocalStorage.DidAlreadyShowBluetoothNote = true;

            var noBleDialog = new ContentDialog
            {
                XamlRoot = Content.XamlRoot,
                Title = "Please note:",
                Content = "Due to technical limitations, sending and receiving files via Bluetooth on Windows is not possible. Please try connecting both devices to the same Wi-Fi network.",
                CloseButtonText = "OK"
            };

            await noBleDialog.ShowAsync();
        }
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

    private async void OnDrop(object sender, DragEventArgs eventArgs)
    {
        eventArgs.AcceptedOperation = DataPackageOperation.Copy;
        DropZone.Background = new SolidColorBrush(Colors.Transparent);

        if (eventArgs.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await eventArgs.DataView.GetStorageItemsAsync();
            var paths = new List<string>();

            foreach (var item in items)
            {
                if (item is StorageFile file)
                {
                    paths.Add(file.Path);
                }
                else if (item is StorageFolder folder)
                {
                    paths.Add(folder.Path);
                }
            }

            ViewModel.SendFiles(paths);
        }
    }
}
