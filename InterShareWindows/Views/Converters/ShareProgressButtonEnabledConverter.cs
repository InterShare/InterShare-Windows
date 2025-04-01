using Microsoft.UI.Xaml.Data;
using System;
using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.UI.Xaml;

namespace InterShareWindows.Views.Converters;

class ShareProgressButtonEnabledConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value is ShareProgressState.Finished)
        {
            return true;
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}