using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using InterShareWindows.Data;
using InterShareWindows.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

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
