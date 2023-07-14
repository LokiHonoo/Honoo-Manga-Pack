using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HonooUI.WPF.Converters
{
    public sealed class DoubleToThicknessTopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                return new Thickness(0, val, 0, 0);
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}