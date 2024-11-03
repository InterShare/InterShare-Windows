using InterShareSdk;
using InterShareWindows.Data;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterShareWindows.Views.Converters;

class ProgressRingIsIndeterminateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var progress = value as SendProgressState;

        if (progress == null)
        {
            return true;
        }

        if (progress is SendProgressState.Transferring or SendProgressState.Finished)
        {
            return false;
        }

        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // This method is not used in this example, but you can implement it if needed
        throw new NotImplementedException();
    }
}
