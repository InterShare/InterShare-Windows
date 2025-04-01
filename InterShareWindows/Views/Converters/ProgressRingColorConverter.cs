using InterShareSdk;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace InterShareWindows.Views.Converters;

public class ProgressRingColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return new SolidColorBrush(Colors.MediumPurple);
        }

        if (progress is SendProgressState.Finished)
        {
            return new SolidColorBrush(Colors.LimeGreen);
        }

        if (progress is SendProgressState.Cancelled or SendProgressState.Declined)
        {
            return new SolidColorBrush(Colors.IndianRed);
        }

        return new SolidColorBrush(Colors.MediumPurple);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}
