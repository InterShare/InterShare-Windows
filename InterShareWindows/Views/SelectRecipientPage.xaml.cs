using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

namespace InterShareWindows.Views;

public sealed partial class SelectRecipientPage : Page
{
    public SelectRecipientViewModel ViewModel
    {
        get;
    }

    public SelectRecipientPage()
    {
        ViewModel = App.GetService<SelectRecipientViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }
}
