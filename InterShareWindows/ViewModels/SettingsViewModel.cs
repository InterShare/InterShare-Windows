// SettingsViewModel.cs

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Sms;
using Windows.Storage;
using ABI.Windows.Security.ExchangeActiveSyncProvisioning;
using CommunityToolkit.Mvvm.Input;
using InterShareWindows.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DeviceInformation = ABI.Windows.Devices.Enumeration.DeviceInformation;

namespace InterShareWindows.ViewModels
{
    public class SettingsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public readonly AsyncRelayCommand SaveCommand;

        private string _deviceName;
        private bool _isShortInput;

        public string DeviceName
        {
            get => _deviceName;
            set => SetProperty(ref _deviceName, value);
        }
        public bool IsShortInput
        {
            get => _isShortInput;
            set => SetProperty(ref _isShortInput, value);
        }
        public SettingsViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCommand = new AsyncRelayCommand(SaveAsync);

            LoadDeviceName();
        }

        private async Task SaveAsync()
        {
            if (DeviceName.Length < 3)
            {
                IsShortInput = true;
                OnPropertyChanged(nameof(IsShortInput));
                return;
            }
            else
            {
                IsShortInput = false;
                OnPropertyChanged(nameof(IsShortInput));
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["DeviceName"] = DeviceName;
            }
        }

        private async Task ShowErrorMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK"
            };
            
            await dialog.ShowAsync();
        }
        
        public async Task LoadDeviceName()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.TryGetValue("DeviceName", out object savedDeviceName))
            {
                DeviceName = savedDeviceName.ToString();
            }
            else
            {
                // Set a default value if the device name is not found in local storage
                DeviceName = System.Environment.MachineName;
            }

            IsShortInput = false;
            OnPropertyChanged(nameof(IsShortInput));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}