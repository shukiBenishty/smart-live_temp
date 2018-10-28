using BE.Models;
using System;
using System.Windows.Data;

namespace GUI.View.Converters
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((parameter == null)? null :  parameter.ToString() )== ((value == null)?null : value.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
          
            if ((bool)value)
            {
                return (GENDER)Enum.Parse(typeof(GENDER), (string)parameter);
            }
            return null;
        }
    }
}
