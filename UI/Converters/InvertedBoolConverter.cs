using System.Globalization;
using System.Windows.Data;

namespace NR155910155992.MemoGame.UI.Converters
{	public class InvertedBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool booleanValue)
			{
				return !booleanValue;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool booleanValue)
			{
				return !booleanValue;
			}
			return value;
		}
	}
}
