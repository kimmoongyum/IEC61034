using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter
{
    public class StatusCodeToDescConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (SessionManager.Current.CurrentLanguage.Equals("KOR"))
            {
                return value.ToString().Equals("T") ? "시험" : "접수";                
            }
            else
            {
                return value.ToString().Equals("T") ? "Test" : "Registration";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
