using System;
using System.Globalization;
using System.Windows.Data;


namespace NoeticTools.TeamStatusBoard.Framework.Converters
{
    internal class DoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var dblValue = (double) value;
            var scale = double.Parse(((string) parameter), CultureInfo.InvariantCulture.NumberFormat);
            return dblValue*scale;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}