using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter
{
    public class DisplaySecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int totalSecond = 0;

            try
            {
                TimeSpan totalSeconds = TimeSpan.FromSeconds(System.Convert.ToInt32(value));

                return totalSeconds.ToString(@"hh\:mm\:ss");
            }
            catch { }

            return totalSecond.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
