﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HonooUI.WPF.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility val)
            {
                return val == Visibility.Visible;
            }
            throw new NotImplementedException();
        }
    }
}