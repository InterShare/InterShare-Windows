using InterShareSdk;
using Microsoft.UI.Xaml.Data;
using System;

namespace InterShareWindows.Views.Converters;

internal partial class ProgressStateTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return "";
        }

        if (progress is SendProgressState.Transferring)
        {
            return "Sending";
        }
        else if (progress is SendProgressState.Connecting)
        {
            return "Connecting";
        }
        else if (progress is SendProgressState.Requesting)
        {
            return "Requesting";
        }
        else if (progress is SendProgressState.Declined)
        {
            return "Declined";
        }
        else if (progress is SendProgressState.Finished)
        {
            return "Finished";
        }
        else if (progress is SendProgressState.Cancelled)
        {
            return "Cancelled";
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}
