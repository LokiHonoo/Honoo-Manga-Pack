using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Honoo.MangaUnpack.Converters
{
    public sealed class InfoButtonStyleConverter : IValueConverter
    {
        public object FalseValue { get; set; } = DependencyProperty.UnsetValue;
        public object TrueValue { get; set; } = DependencyProperty.UnsetValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hasFailed)
            {
                return hasFailed ? TrueValue : FalseValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}