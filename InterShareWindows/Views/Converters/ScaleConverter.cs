﻿using InterShareSdk;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShareWindows.Views.Converters;

internal class ScaleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return new ScaleTransform() { ScaleX = 1.0, ScaleY = 1.0 };
        }

        if (progress
            is SendProgressState.Transferring
            or SendProgressState.Finished
            or SendProgressState.Connecting
            or SendProgressState.Compressing
            or SendProgressState.Requesting)
        {
            return new ScaleTransform() { ScaleX = 0.7, ScaleY = 0.7 };
        }

        return new ScaleTransform() { ScaleX = 1.0, ScaleY = 1.0 };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}