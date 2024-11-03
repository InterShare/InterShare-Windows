using System;
using Windows.Storage;
using ABI.Windows.Foundation;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace InterShareWindows.Data;

internal record SettingsFile
{
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public bool DidAlreadyShowBluetoothNote { get; set; }
}

public static class LocalStorage
{
    public static string SettingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(), "InterShare", "settings.json").ToString();

    private static SettingsFile _currentSettings;

    private static SettingsFile CurrentSettings
    {
        get
        {
            return GetSettings();
        }
        set
        {
            if (_currentSettings == null)
            {
                GetSettings();
            }

            _currentSettings = value;
            SaveSettings();
        }
    }

    private static SettingsFile GetSettings()
    {
        if (_currentSettings == null)
        {
            if (!File.Exists(SettingsPath))
            {
                _currentSettings = new SettingsFile
                {
                    DeviceId = Guid.NewGuid().ToString(),
                    DeviceName = Environment.MachineName,
                    DidAlreadyShowBluetoothNote = false
                };

                SaveSettings();
            }
            else
            {
                var file = File.OpenRead(SettingsPath);
                var settings = JsonSerializer.Deserialize<SettingsFile>(file);
                _currentSettings = settings;
            }
        }


        return _currentSettings;
    }

    private static void SaveSettings()
    {
        var serialized = JsonSerializer.Serialize(_currentSettings);
        File.WriteAllText(SettingsPath, serialized);
    }
    
    public static string DeviceId
    {
        get
        {   
            return CurrentSettings.DeviceId;
        }
    }
    
    public static string DeviceName
    {
        get
        {
            //if (LocalSettings.Values.TryGetValue(DeviceNameKey, out var value))
            //{
            //    return (string)value;
            //}
            
            return CurrentSettings.DeviceName;
        }
        
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            CurrentSettings.DeviceName = value.Trim();
            SaveSettings();
        }
    }

    public static bool DidAlreadyShowBluetoothNote
    {
        get
        {
            return CurrentSettings.DidAlreadyShowBluetoothNote;
        }

        set
        {
            CurrentSettings.DidAlreadyShowBluetoothNote = value;
            SaveSettings();
        }
    }
}