using AvanadeStudioTV.ViewModels;
using AvanadeStudioTV.Views;
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
	public partial class SettingsPage : ContentPage
	{
	   public SettingsPageViewModel ViewModel;
		public SettingsPage (MasterPage master, INavigation nav)
		{
			InitializeComponent ();

		   ViewModel = new SettingsPageViewModel(nav, master);
			this.BindingContext = ViewModel;

		}
	}
}