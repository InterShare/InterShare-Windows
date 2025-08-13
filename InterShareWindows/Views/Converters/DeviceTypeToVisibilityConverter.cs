using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace InterShareWindows.Views.Converters;

public class DeviceTypeToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null || parameter == null)
            return Visibility.Collapsed;
        
        var deviceType = value.ToString();
        var targetDeviceType = parameter.ToString();

        return deviceType.Equals(targetDeviceType, StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;

    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}