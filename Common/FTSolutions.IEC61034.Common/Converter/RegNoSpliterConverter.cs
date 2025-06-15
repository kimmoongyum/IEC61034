using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter
{
    public class RegNoSpliterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] regInfo = value.ToString().Split(new char[] { '_' });

            if (regInfo.Length == 2)
            {
                if ("0".Equals(parameter.ToString()))
                {
                    return regInfo[0];
                }
                else
                {
                    return regInfo[1];
                }
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
