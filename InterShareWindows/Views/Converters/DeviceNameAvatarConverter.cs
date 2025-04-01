using System;
using Microsoft.UI.Xaml.Data;

namespace InterShareWindows.Views.Converters;

public class DeviceNameAvatarConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null)
            return "";

        return ((string) value)[0].ToString().ToUpper();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}
