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
using System.Timers;

namespace AvanadeStudioTV.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FullScreenListPage : ContentPage
	{
		public FullScreenListViewViewModel ViewModel;
		private bool UseDisplayTimeout;
		public System.Timers.Timer DisplayTimoutTimer;

		public FullScreenListPage(bool useDisplayTimeout)
		{
			UseDisplayTimeout = useDisplayTimeout;

			InitializeComponent();


			ViewModel = new FullScreenListViewViewModel(this.Navigation);

			if (UseDisplayTimeout)
			{
				DisplayTimoutTimer = new System.Timers.Timer();
				DisplayTimoutTimer.Interval = App.DataManager.INTERSTITAL_SCREEN_DISPLAY_INTERVAL; // 5 second  
				DisplayTimoutTimer.Elapsed += DisplayTimoutTimer_Elapsed;
				DisplayTimoutTimer.Start();
			}

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (ViewModel == null ) ViewModel = new FullScreenListViewViewModel(this.Navigation);
 

			this.BindingContext = ViewModel;


			ViewModel.SetupWeather();



			var timer = new System.Timers.Timer();
			timer.Interval = 1000;// 1 second  
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			this.BindingContext = null;

			
			 
		}

		private void DisplayTimoutTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			

			Device.BeginInvokeOnMainThread(() =>
			{
				DisplayTimoutTimer.Stop();
				DisplayTimoutTimer.Dispose();
				 if (Navigation.ModalStack.Count > 0)
				Navigation.PopModalAsync(true);
			});
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