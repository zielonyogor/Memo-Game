using System.Globalization;
using System.Windows.Data;

namespace NR155910155992.MemoGame.UI.Converters
{
    public class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
                return i.ToString(culture);
            if (value is int?)
            {
                var n = (int?)value;
                return n.HasValue ? n.Value.ToString(culture) : string.Empty;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s))
                return null;
            if (int.TryParse(s, NumberStyles.Integer, culture, out var i))
                return i;
            return null;
        }
    }
}
