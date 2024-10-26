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

namespace InterShareWindows.ViewModels
{
    public class SettingsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public readonly RelayCommand SaveCommand;

        private string _deviceName;
        private bool _showErrorDeviceNameToShort;

        public string DeviceName
        {
            get => _deviceName;
            set
            {
                SetProperty(ref _deviceName, value);
                OnPropertyChanged(nameof(DeviceName));
            }
        }

        public bool ShowErrorDeviceNameToShort
        {
            get => _showErrorDeviceNameToShort;
            set
            {
                SetProperty(ref _showErrorDeviceNameToShort, value);
                OnPropertyChanged(nameof(ShowErrorDeviceNameToShort));
            }
        }

        public SettingsViewModel(INavigationService navigationService) : base(navigationService)
        {
            DeviceName = LocalStorage.DeviceName;
            
            SaveCommand = new RelayCommand(Save);
        }

        private void Save()
        {
            try
            {
                LocalStorage.DeviceName = DeviceName;
                
                DeviceName = DeviceName.Trim();
            }
            catch (ArgumentException e)
            {
                ShowErrorDeviceNameToShort = true;
            }
        }
        
        public new event PropertyChangedEventHandler PropertyChanged;
        
        protected override void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}