using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AvanadeStudioTV.Views.Controls
{
	/// <summary>
	/// NOTE: Both of these converters are not used IN CURRENT CODE:
	/// </summary>
	public class StringToUpperAndWideSpacingConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value != null && value is string)
		{
			var s = ((string)value).ToUpper();
			return s.Aggregate(string.Empty, (c, i) => c + i + ' ');
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}


	class EqualsParameterContextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == ((View)parameter).BindingContext;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
