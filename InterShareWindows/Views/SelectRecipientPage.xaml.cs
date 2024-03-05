using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using InterShareWindows.Params;
using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
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
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var sendParam = e.Parameter;
        if (sendParam is null)
        {
            ViewModel.GoBackCommand.Execute(null);
            return;
        }

        ViewModel.SendParam = e.Parameter as SendParam;
    }
}
