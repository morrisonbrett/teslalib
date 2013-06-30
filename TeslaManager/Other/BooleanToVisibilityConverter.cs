using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeslaManager.Other
{
    /// <summary>
    /// Converter to convert from bool to Visibility, and vice versa
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Converts from bool to Visibility
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isReversed = false;
            
            if (parameter != null && parameter is string)
            {
               bool.TryParse((string)parameter, out isReversed);
            }

            if (value == null || !(value is bool))
                return Visibility.Collapsed;

            bool boolValue = (bool)value;

            if (isReversed)
            {
                boolValue = !boolValue;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts from Visibility to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Visibility))
            {
                return true;
            }

            bool isReversed = false;

            if (parameter != null && parameter is string)
            {
                bool.TryParse((string)parameter, out isReversed);
            }

            Visibility visibility = (Visibility)value;

            bool result = visibility == Visibility.Visible;

            if (isReversed)
            {
                result = !result;
            }

            return result;
        }
    }
}
