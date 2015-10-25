using System;
using System.Globalization;
using System.Windows.Data;

namespace NoeticTools.TeamDashboard.Converters
{
    internal class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var dblValue = (double) value;
            double scale = Double.Parse(((string) parameter), CultureInfo.InvariantCulture.NumberFormat);
            return dblValue*scale;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}