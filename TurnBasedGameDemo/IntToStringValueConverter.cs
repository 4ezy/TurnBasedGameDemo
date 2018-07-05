using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TurnBasedGameDemo
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool convRes = int.TryParse((string)value, out int result);

            if (!convRes)
                return 0;
            else if (result < 0)
                return 0;
            else if (result > 20)
                return 20;
            else
                return result;
        }
    }
}
