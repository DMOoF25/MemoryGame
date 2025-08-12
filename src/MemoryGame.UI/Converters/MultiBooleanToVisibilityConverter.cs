using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MemoryGame.UI.Converters;

public class MultiBooleanToVisibilityConverter : IMultiValueConverter
{
    public bool UseOrLogic { get; set; } = false;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var bools = values.OfType<bool>().ToArray();
        bool result = UseOrLogic ? bools.Any(b => b) : bools.All(b => b);
        return result ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
