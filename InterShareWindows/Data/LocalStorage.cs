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
    public static string SettingsFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString(), "InterShare").ToString();
    public static string SettingsFilePath => Path.Combine(SettingsFolderPath, "settings.json").ToString();

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
            if (!Directory.Exists(SettingsFolderPath))
            {
                Directory.CreateDirectory(SettingsFolderPath);
            }

            if (!File.Exists(SettingsFilePath))
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
                using var file = File.OpenRead(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<SettingsFile>(file);
                _currentSettings = settings;
            }
        }


        return _currentSettings;
    }

    private static void SaveSettings()
    {
        var serialized = JsonSerializer.Serialize(_currentSettings);
        File.WriteAllText(SettingsFilePath, serialized);
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