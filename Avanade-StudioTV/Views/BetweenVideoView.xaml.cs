using Avanade_StudioTV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvanadeStudioTV.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BetweenVideoView : ContentView
	{

		public ListView FeedView
		{
			get
			{
				return FeedListView;
			}
		}
		public BetweenVideoView ()
		{


			InitializeComponent ();

			var timer = new System.Timers.Timer();
			timer.Interval = 1000;// 1 second  
			timer.Elapsed += Clock_Timer_Elapsed;
			timer.Start();

		}



		private void Clock_Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				ClockLabel.Text = string.Format("{0:h:mm:ss tt}", DateTime.Now);
			});
		}

	
	 
	}
}