using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

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

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        await Task.CompletedTask;
    }
}
