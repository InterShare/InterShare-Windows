using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml.Controls;

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
}
