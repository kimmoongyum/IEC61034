using System;
using System.Globalization;
using System.Windows.Data;

namespace FTSolutions.IEC61034.Common.Converter 
{
    public class StepMotorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine(string.Format("Convert -> {0} : {1}", value.ToString(), parameter.ToString()));
            if (value != null && parameter != null)
            {
                return parameter.ToString().ToUpper().Equals(value.ToString().ToUpper());
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
