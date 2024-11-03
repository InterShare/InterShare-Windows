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

namespace InterShareWindows.ViewModels
{
    public partial class SettingsViewModel : ViewModelBase
    {
        public readonly RelayCommand SaveCommand;

        [ObservableProperty]
        private string _deviceName;

        [ObservableProperty]
        private bool _showErrorDeviceNameToShort;

        [ObservableProperty]
        private string _version;

        public SettingsViewModel() : base()
        {
            DeviceName = LocalStorage.DeviceName;

            var version = Assembly.GetExecutingAssembly()
                                  .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                  ?.InformationalVersion;
            Version = version ?? "Version not found";

            SaveCommand = new RelayCommand(Save);
        }

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
            }
            catch (ArgumentException e)
            {
                ShowErrorDeviceNameToShort = true;
            }
        }
    }
}