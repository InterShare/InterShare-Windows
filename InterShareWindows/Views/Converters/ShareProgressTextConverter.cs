using Microsoft.UI.Xaml.Data;
using System;
using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.UI.Xaml;

namespace InterShareWindows.Views.Converters;

class ShareProgressTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            if (value is ShareProgressState.Compressing compressingState)
            {
                return $"Compressing {compressingState.progress:P2}";
            }
            
            if (value is ShareProgressState.Finished)
            {
                return $"Compressing {1.0:P2}";
            }
        }

        return $"Compressing {0.0:P2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}