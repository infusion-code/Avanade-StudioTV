using System;
using System.Collections.Generic;
using Xamarin.Forms;

 
using System.Globalization;

using System.Linq;

namespace AvanadeStudioTV.Views
{

    public partial class BigFeedCell : ViewCell
    {

        public BigFeedCell()
        {
            InitializeComponent();

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
		
 
        }
    }


	/*public class StringToUpperAndWideSpacingConverter : IValueConverter
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
	} */
}
