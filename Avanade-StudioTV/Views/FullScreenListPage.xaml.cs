using AvanadeStudioTV.ViewModels;
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
		public FullScreenListViewViewModel ViewModel;

		public FullScreenListPage ()
		{
			InitializeComponent ();
			ViewModel = new FullScreenListViewViewModel(this.Navigation);
			this.BindingContext = ViewModel;
		}

		private void CloseButton_Clicked(object sender, EventArgs e)
		{
			this.Navigation.PopModalAsync();
		}
	}
}