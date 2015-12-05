using System;
using System.Globalization;
using System.Windows.Data;


namespace NoeticTools.Dashboard.Framework.Plugins.PropertyEditControls
{
    /// <summary>
    /// Convert between UTC file time (long) and displayed date text.
    /// </summary>
    public class UtcDateTimeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof (string))
            {
                if (value is DateTime)
                {
                    var date = (DateTime) value;
                    return date.ToLocalTime();
                }

                var text = value as string;
                if (string.IsNullOrWhiteSpace(text))
                {
                    return new DateTime(2000, 1, 1);
                }

                return DateTime.FromFileTimeUtc(long.Parse(text));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return "0";
            }

            var date = DateTime.Parse(text, CultureInfo.CurrentCulture).ToUniversalTime();
            if (targetType == typeof (DateTime))
            {
                return date;
            }

            if (targetType == typeof (object))
            {
                return date.ToFileTime().ToString();
            }

            return null;
        }
    }
}