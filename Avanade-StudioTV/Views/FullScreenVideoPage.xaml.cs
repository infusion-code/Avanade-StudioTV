using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xam.Plugin.WebView;
using Xam.Plugin.WebView.Abstractions;

using FormsVideoLibrary;

using Xamarin.Forms;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.ViewModels;
using System.Timers;
using System.Threading.Tasks;

namespace AvanadeStudioTV.Views
{
    public partial class FullScreenVideoPage : ContentPage
    {
        public event VideoCompletedHandler VideoCompleted;
        public delegate void VideoCompletedHandler();
		public FullScreenVideoViewModel ViewModel;
		public Timer ShowTitleTimer;
		public Timer HideTitleTimer;
		public const double TITLE_VISIBLE_SCREEN_TIME = 10000; //10 sec
		public const double TITLE_HIDDEN_SCREEN_TIME = 5000; //8 sec



		public string Source { get; set; }
		public bool IsInCurrentTitleMode { get; private set; }



		/// <summary>
		/// UWP Full screen desktop video view (uses native UWP Media Element to play videos)
		/// </summary>
		/// <param name="source"></param>
		public FullScreenVideoPage ()
        {
            Source = "http://video.ch9.ms/ch9/e38f/1c70afc7-8af1-459b-be04-bb3b6eeee38f/OnNETAzureB2CAD.mp4";


			InitializeComponent();

	 


			ViewModel = new FullScreenVideoViewModel(Navigation, this);
			this.BindingContext = ViewModel;

			StartAnimatingTitles();



		}



		private void LoadNextVideo(string obj)
        {
            if (VideoCompleted != null)
            {
                VideoCompleted();
            }
        }

        public void ResetEvents()
        {
            VideoCompleted = delegate { };
        }


        private void VideoWebView_OnContentLoaded(object sender, EventArgs e)
        {
            PlayVideo("AvanadeStudioIntro.mp4");
            
        }

		internal void PlayIntro()
		{
			 
				VideoPlayerView.Source = VideoSource.FromResource("AvanadeStudioIntro.mp4");

				VideoPlayerView.Play();
				this.ForceLayout();

				VideoPlayerView.VideoEnded += VideoPlayerView_IntroVideoEnded;
 
		}

		private void VideoPlayerView_IntroVideoEnded(object sender, EventArgs e)
		{
			VideoPlayerView.VideoEnded -= VideoPlayerView_IntroVideoEnded;

			VideoPlayerView.Source = VideoSource.FromUri(Source);
			VideoPlayerView.Play();
			this.ForceLayout();

			VideoPlayerView.VideoEnded += VideoPlayerView_VideoEnded;
		}

		
		private void VideoPlayerView_VideoEnded(object sender, EventArgs e)
		{
			
			VideoPlayerView.VideoEnded -= VideoPlayerView_VideoEnded;
			VideoCompleted?.Invoke();
		}

		/// <summary>
		/// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
		/// </summary>
		/// <remarks>
		/// To be added.
		/// </remarks>
		protected override void OnAppearing()
        {
            base.OnAppearing();

			IsInCurrentTitleMode = true;


				VideoPlayerView.HeightRequest = MainGrid.Height;
				VideoPlayerView.WidthRequest = MainGrid.WidthRequest;
				VideoPlayerView.VerticalOptions = LayoutOptions.FillAndExpand;

			PlayVideo(this.Source);

			StartAnimatingTitles();

		}

		private void StartAnimatingTitles()
		{
			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;
			SetupTimers();
			StartShowTimer();
		}

		private void StopAnimatingTitles()
		{
			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;

			ShowTitleTimer.Stop();
			HideTitleTimer.Stop();
		}

		private void SetupTimers()
		{
			//Show Title
			ShowTitleTimer = new System.Timers.Timer();
			ShowTitleTimer.Elapsed += new ElapsedEventHandler(_ShowTitle_timer_Elapsed);
			//5 second inteval
			ShowTitleTimer.Interval = TITLE_VISIBLE_SCREEN_TIME;

			HideTitleTimer = new System.Timers.Timer();
			HideTitleTimer.Elapsed += new ElapsedEventHandler(_HideTitle_timer_Elapsed);
			//1 second
			HideTitleTimer.Interval = TITLE_HIDDEN_SCREEN_TIME;

		}

		private void StartHideTimer()
		{

			HideTitleTimer.Start();
		}

		private void StartShowTimer()
		{
			
			ShowTitleTimer.Start();
		}

		private async void _ShowTitle_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await ShowView();			
			ShowTitleTimer.Stop();
			StartHideTimer();
		}

		private async Task ShowView()
		{
			if (IsInCurrentTitleMode)

			{
				await TitleView.FadeTo(1, 2000, Easing.Linear);
			}

			else await NextShowView.FadeTo(1, 2000, Easing.Linear);
 
		}

		private async void _HideTitle_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await HideView();
			
			HideTitleTimer.Stop();
			IsInCurrentTitleMode = !IsInCurrentTitleMode; //switch from current title to next up title
			StartShowTimer();
		}

		private async Task HideView()
		{
			if (IsInCurrentTitleMode)

			{
				await TitleView.FadeTo(0, 2000, Easing.Linear);
			}

			else await NextShowView.FadeTo(0, 2000, Easing.Linear);

		}

		public async void PlayVideo(string url)
        {
            Source = url;
            String pv = "PlayVideo('" + Source + "');";
            Device.BeginInvokeOnMainThread(() =>
            {
				 
					
					VideoPlayerView.VideoEnded -= VideoPlayerView_VideoEnded;
					PlayIntro();
					
				 
			});
   
             
        }

	 

	

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
			StopAnimatingTitles();

		}

		private void TitleView_Tapped(object sender, EventArgs e)
		{
		

			if (IsInCurrentTitleMode)
			{
				//OpenFullScreenListPage()
				OpenSettingsPage();
			}

			else ShowNextVideo();


		}

		private void ShowNextVideo()
		{
		 
			ViewModel.VideoPage_VideoCompleted();
			ViewModel.StartVideo();
			

		}

		private void OpenFullScreenListPage()
		{
			var listPage = new FullScreenListPage();
			this.Navigation.PushModalAsync(listPage);

		}

		private void OpenSettingsPage()
		{
			var settings = new SettingsPage(this.Navigation);
			this.Navigation.PushModalAsync(settings);

		}
	}
}
