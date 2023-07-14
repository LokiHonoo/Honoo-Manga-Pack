using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HonooUI.WPF.Converters
{
    public sealed class DropArgsToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values[0] is bool executeAtDrop && values[1] is bool running)
            {
                if (!executeAtDrop)
                {
                    return Visibility.Visible;
                }
                else if (running)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}