﻿using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShareWindows.Views.Converters;
class ProgressRingActiveConverter : IValueConverter
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
            or SendProgressState.Connecting
            or SendProgressState.Compressing
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