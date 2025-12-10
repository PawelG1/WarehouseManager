using System;
using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagerApp.Converters
{
    public class PercentageToWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double percentage && values[1] is double totalWidth)
            {
                // Calculate width based on percentage (0-100) and total available width
                var width = (percentage / 100.0) * totalWidth;
                return Math.Max(0, Math.Min(width, totalWidth)); // Clamp between 0 and totalWidth
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
