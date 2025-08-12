﻿using System.Globalization;
using System.Windows.Data;

namespace MemoryGame.UI.Converters;

public class BoolInverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b ? !b : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
