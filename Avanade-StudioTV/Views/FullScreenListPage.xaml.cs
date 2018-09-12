using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvanadeStudioTV.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FullScreenListPage : ContentPage
	{
		public FullScreenListPage ()
		{
			InitializeComponent ();
		}

		private void CloseButton_Clicked(object sender, EventArgs e)
		{
			this.Navigation.PopModalAsync();
		}
	}
}