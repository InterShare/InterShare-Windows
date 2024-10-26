using System;
using Windows.Storage;
using ABI.Windows.Foundation;

namespace InterShareWindows.Data;

public static class LocalStorage
{
    private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

    private const string DeviceIdKey = "device_id";
    private const string DeviceNameKey = "device_name";
    
    public static Guid DeviceId
    {
        get
        {
            if (LocalSettings.Values.TryGetValue(DeviceIdKey, out var value))
            {
                return Guid.Parse((string)value);
            }

            var deviceId = Guid.NewGuid();
            LocalSettings.Values[DeviceIdKey] = deviceId;
            
            return deviceId;
        }
    }
    
    public static string DeviceName
    {
        get
        {
            if (LocalSettings.Values.TryGetValue(DeviceNameKey, out var value))
            {
                return (string)value;
            }
            
            return Environment.MachineName;
        }
        
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            
            LocalSettings.Values[DeviceNameKey] = value.Trim();
        }
    }
    
}