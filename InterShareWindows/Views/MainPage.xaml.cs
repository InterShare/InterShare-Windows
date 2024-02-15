using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using InterShareWindows.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace InterShareWindows.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
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
