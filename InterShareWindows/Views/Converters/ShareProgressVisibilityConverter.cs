using Microsoft.UI.Xaml.Data;
using System;
using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.UI.Xaml;

namespace InterShareWindows.Views.Converters;

class ShareProgressIsActiveVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            if (value is ShareProgressState.Finished)
            {
                return Visibility.Collapsed;
            }
            
            if (value is ShareProgressState.Compressing compressingState)
            {
                if (Math.Abs(compressingState.progress - 1.0) < 0.0001)
                {
                    return Visibility.Collapsed;
                }
            }
            
            return Visibility.Visible;
        }
        
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}

class ShareProgressIsInactiveVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        if (value is not null)
        {
            if (value is ShareProgressState.Finished)
            {
                return Visibility.Visible;
            }
            
            if (value is ShareProgressState.Compressing compressingState)
            {
                if (Math.Abs(compressingState.progress - 1.0) < 0.0001)
                {
                    return Visibility.Visible;
                }
            }
            
            return Visibility.Collapsed;
        }
        
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}