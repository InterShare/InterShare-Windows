using InterShareSdk;
using Microsoft.UI.Xaml.Data;
using System;

namespace InterShareWindows.Views.Converters;

internal partial class ProgressRingActiveConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return false;
        }

        if (progress
            is SendProgressState.Transferring
            or SendProgressState.Finished
            or SendProgressState.Cancelled
            or SendProgressState.Declined
            or SendProgressState.Connecting
            or SendProgressState.Requesting)
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
