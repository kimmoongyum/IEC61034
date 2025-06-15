using eccFramework.SharedLib.GlobalType.SysType;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter
{
    public class BooleanToValidColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? GlobalConst.VALID_BLUSH : GlobalConst.INVALID_BLUSH;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
