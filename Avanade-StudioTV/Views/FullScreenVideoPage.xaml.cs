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
		public const double TITLE_VISIBLE_SCREEN_TIME = 5000; //10 sec
	 



		public string Source { get; set; }
		public bool IsInCurrentTitleMode { get; private set; }
		public bool ShowAnimations { get;   set; }



		/// <summary>
		/// UWP Full screen desktop video view (uses native UWP Media Element to play videos)
		/// </summary>
		/// <param name="source"></param>
		public FullScreenVideoPage ()
        {
             


			InitializeComponent();

			IsInCurrentTitleMode = true;


			ViewModel = new FullScreenVideoViewModel(Navigation, this);

			//First Time Launching Get Feed Data from Network First:
			ViewModel.GetNewsFeedFromNetworkAsync();
			this.BindingContext = ViewModel;

			



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

			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;
			 ShowAnimations = true;
			ShowAnimatedViews();

		}

		public void ResetEvents()
        {
            VideoCompleted = delegate { };
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



		public void StartAnimatingTitles()
		{
			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;
			SetupTimers();
			
		}

		public void StopAnimatingTitles()
		{
			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;

			ViewExtensions.CancelAnimations(TitleView);
			ViewExtensions.CancelAnimations(NextShowView);




		}

		private void SetupTimers()
		{
			//Show Title
			ShowTitleTimer = new System.Timers.Timer();
			ShowTitleTimer.Elapsed += new ElapsedEventHandler(_ShowTitle_timer_Elapsed);
			//5 second inteval
			ShowTitleTimer.Interval = 5000;
			ShowTitleTimer.Start();

		}

	 

		private async void _ShowTitle_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await ShowAnimatedViews();			
 
		}

		public async Task ShowAnimatedViews()
		{
			while (ShowAnimations)
			{

				await TitleView.FadeTo(1, 2000, Easing.Linear);
				await TitleView.FadeTo(1, 10000, Easing.Linear); //stay for 10 sec
				await TitleView.FadeTo(0, 2000, Easing.Linear);

				await TitleView.FadeTo(0, 2000, Easing.Linear); //wait for 2 sec

				await NextShowView.FadeTo(1, 2000, Easing.Linear);
				await NextShowView.FadeTo(1, 5000, Easing.Linear);
				await NextShowView.FadeTo(0, 2000, Easing.Linear);

				await NextShowView.FadeTo(0, 2, Easing.Linear); //wait 2 seconds

			}

		}
 
 

 

		public  void PlayVideo(string url)
        {
            Source = url;
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

			OpenFullScreenListPageAsync();
		}


		private void NextVideoView_Tapped(object sender, EventArgs e)
		{
			OpenFullScreenListPageAsync();
			//
			//		ViewModel.VideoPage_VideoCompleted();
			//	ViewModel.StartVideo();


		}

 

		private async Task OpenFullScreenListPageAsync()
		{
			VideoPlayerView.Stop();

			var modalPage = new FullScreenListPage(false);
			await Navigation.PushModalAsync(modalPage, true);
		 
		}


		//not used on full screen page
		private void OpenSettingsPage()
		{
			var settings = new SettingsPage(this.Navigation);
			this.Navigation.PushModalAsync(settings);

		}
	}
}
