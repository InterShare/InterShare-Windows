// SettingsViewModel.cs

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sms;
using Windows.Storage;
using ABI.Windows.Security.ExchangeActiveSyncProvisioning;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Data;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DeviceInformation = ABI.Windows.Devices.Enumeration.DeviceInformation;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InterShareWindows.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly UpdateService _updateService;

    [ObservableProperty]
    private string _deviceName;

    [ObservableProperty]
    private bool _showErrorDeviceNameToShort;

    [ObservableProperty]
    private bool _updateButtonEnabled = true;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private bool _noUpdatesFoundTextVisible;

    public SettingsViewModel(NavigationService navigationService, UpdateService updateService) : base()
    {
        _navigationService = navigationService;
        _updateService = updateService;

        DeviceName = LocalStorage.DeviceName;

        var version = Assembly.GetExecutingAssembly()
                              .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                              ?.InformationalVersion;

        var semVer = Regex.Match(version ?? string.Empty, @"^[0-9]+\.[0-9]+\.[0-9]+(?:-[\w\d\-.]+)?").Value;
        Version = semVer != string.Empty ? semVer : "Version not found";
    }

    [RelayCommand]
    private void Save()
    {
        try
        {
            if (DeviceName.Trim().Length < 1)
            {
                ShowErrorDeviceNameToShort = true;
                return;
            }

            ShowErrorDeviceNameToShort = false;
            LocalStorage.DeviceName = DeviceName;

            DeviceName = DeviceName.Trim();

            _navigationService.GoBack();

        }
        catch (ArgumentException e)
        {
            ShowErrorDeviceNameToShort = true;
        }
    }

    [RelayCommand]
    private async Task Update()
    {
        NoUpdatesFoundTextVisible = false;
        UpdateButtonEnabled = false;
        var updateFound = await _updateService.Update();
        NoUpdatesFoundTextVisible = !updateFound;
        UpdateButtonEnabled = true;
    }
}