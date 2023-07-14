using System;
using System.Globalization;
using System.Windows.Data;

namespace HonooUI.WPF.Converters
{
    public sealed class BooleanToCustumConverter : IValueConverter
    {
        public object FalseValue { get; set; } = new();
        public object TrueValue { get; set; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val ? TrueValue : FalseValue;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == TrueValue;
        }
    }
}