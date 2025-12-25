using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NR155910155992.MemoGame.UI.Converters
{
	class BooleanToVisibilityConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool booleanValue)
			{
				return booleanValue ? Visibility.Visible : Visibility.Hidden;
			}
			return Visibility.Hidden;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility visibilityValue)
			{
				return visibilityValue == Visibility.Visible;
			}
			return false;
		}
	}
}
