using System;
using System.Globalization;
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
            var isReversed = false;
            
            if (parameter != null && parameter is string)
            {
               bool.TryParse((string)parameter, out isReversed);
            }

            if (value == null || !(value is bool))
                return Visibility.Collapsed;

            var boolValue = (bool)value;

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

            var isReversed = false;

            if (parameter != null && parameter is string)
            {
                bool.TryParse((string)parameter, out isReversed);
            }

            var visibility = (Visibility)value;

            var result = visibility == Visibility.Visible;

            if (isReversed)
            {
                result = !result;
            }

            return result;
        }
    }
}
