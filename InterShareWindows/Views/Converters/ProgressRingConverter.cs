﻿using InterShareSdk;
using Microsoft.UI.Xaml.Data;
using System;

namespace InterShareWindows.Views.Converters;

class ProgressRingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return 0.0;
        }

        if (progress is SendProgressState.Transferring)
        {
            var transferringState = progress as SendProgressState.Transferring;
            return transferringState.progress * 100;
        }
        if (progress
            is SendProgressState.Finished
            or SendProgressState.Cancelled
            or SendProgressState.Declined)
        {
            return 100.0;
        }

        return 0.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}
