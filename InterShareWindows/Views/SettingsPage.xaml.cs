using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace InterShareWindows.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        DataContext = ViewModel;
        ViewModel.NoUpdatesFoundTextVisible = false;
        
        InitializeComponent();
    }

    private void DeviceNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ViewModel.DeviceName))
        {
            ViewModel.ShowErrorDeviceNameToShort = false;
        }
    }
}
