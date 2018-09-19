using Avanade_StudioTV;
using AvanadeStudioTV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvanadeStudioTV.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FullScreenListPage : ContentPage
	{
		public FullScreenListViewViewModel ViewModel;
		 

		public FullScreenListPage()
		{
			InitializeComponent();
			ViewModel = new FullScreenListViewViewModel(this.Navigation);
			

			MessagingCenter.Subscribe<string>(this, "WeatherUpdated", (obj) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{

					ViewModel.SetupWeather();
					this.ForceLayout();
				});
			});

 

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			this.BindingContext = ViewModel;


			var timer = new System.Timers.Timer();
			timer.Interval = 1000;// 1 second  
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				ClockLabel.Text = string.Format("{0:h:mm:ss tt}", DateTime.Now);
			});
		}

		private void CloseButton_Clicked(object sender, EventArgs e)
		{
			this.Navigation.PopModalAsync();
		}
	}
}