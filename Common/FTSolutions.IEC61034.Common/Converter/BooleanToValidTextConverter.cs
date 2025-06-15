using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter
{
    public class BooleanToValidTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (SessionManager.Current.CurrentLanguage.Equals("KOR"))
            {
                return ((bool)value) ? "적합" : "부적합";
            }
            else
            {
                return ((bool)value) ? "Valid" : "Invalid";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
