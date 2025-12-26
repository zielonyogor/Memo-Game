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
		public bool Invert { get; set; }

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool res = value is bool b && b;
			if(parameter is string p && bool.TryParse(p, out var invert))
			{
				res = invert ? !res : res;
			}
			else if (Invert)
			{
				res = !res;
			}
			return res ? Visibility.Visible : Visibility.Collapsed;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException();
	}
}
