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
using System.Threading;

namespace InterShareWindows.Views;

public sealed partial class SelectRecipientPage : Page
{
    private bool _isDialogOpen = false;
    private readonly SynchronizationContext _uiContext;

    public SelectRecipientViewModel ViewModel { get; }
    
    public SelectRecipientPage()
    {
        _uiContext = SynchronizationContext.Current;
        ViewModel = App.GetService<SelectRecipientViewModel>();
        ViewModel.ShowBleNotAvailableDialog += ShowBleNotAvailableDialog;
        DataContext = ViewModel;
        ViewModel.Reset();
        InitializeComponent();
    }

    private void ShowBleNotAvailableDialog()
    {
        if (_isDialogOpen)
        {
            return;
        }

        _isDialogOpen = true;

        _uiContext.Post(async _ => {
            try
            {
                ContentDialog noBleDialog = new ContentDialog
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Bluetooth not supported on Windows :(",
                    Content = "Due to technical limitations, sending files via Bluetooth on Windows is not possible. Please try connecting the receiver to the same Wi-Fi network.",
                    CloseButtonText = "OK"
                };

                await noBleDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error showing ContentDialog: {ex}");
            }
            finally
            {
                _isDialogOpen = false;
            }
        }, null);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        if (e.NavigationMode == NavigationMode.Back)
        {
            ViewModel.Stop();
        }
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var sendParam = e.Parameter;
        if (sendParam is null)
        {
            return;
        }

        ViewModel.SendParam = e.Parameter as SendParam;
    }
}
