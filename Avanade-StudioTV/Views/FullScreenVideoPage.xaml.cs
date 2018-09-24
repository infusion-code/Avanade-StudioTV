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
using Xamarin.Essentials;
using System.Threading;
using AvanadeStudioTV.Database;
using System.Linq;

namespace AvanadeStudioTV.Views
{
    public partial class FullScreenVideoPage : ContentPage
    {
        public event VideoCompletedHandler VideoCompleted;
        public delegate void VideoCompletedHandler();
		public FullScreenVideoViewModel ViewModel;
	
		public const double TITLE_VISIBLE_SCREEN_TIME = 5000; //10 sec
	 
		public HiddenViewState HiddenViewStatus;
		public TitleAnimationStatus TitleAnimationState;
	 



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



			HiddenView.FeedView.ItemSelected += FeedListView_ItemSelected;
			//HiddenView.FeedView.ItemAppearing += FeedView_ItemAppearing;
	


		}

		//private void FeedView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		//{
		//	if (ViewModel?.SharedData.NextItem != null)
		//	{
		//		if (ViewModel.SharedData.FeedList != null)
		//		{
		//			int i = ViewModel.SharedData.FeedList.IndexOf(ViewModel.SharedData.SelectedItem);
		//			ViewModel.SharedData.FeedList.ToList().ForEach(c => c.IsSelected = false);
		//			ViewModel.SharedData.FeedList[i].IsSelected = true;
		//			//	FeedListView.ScrollTo(ViewModel.SharedData.FeedList[i], ScrollToPosition.Start, true);
		//		}
		//	}
			 
		//}

		/// <summary>
		/// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
		/// </summary>
		/// <remarks>
		/// To be added.
		/// </remarks>
		protected override void OnAppearing()
		{
			var metrics = DeviceDisplay.ScreenMetrics;
			base.OnAppearing();
	
		   HiddenView.WidthRequest = metrics.Width;
			HiddenView.HeightRequest = metrics.Height;

			ViewModel = new FullScreenVideoViewModel(Navigation, this);
			this.BindingContext = ViewModel;

			


			VideoPlayerView.HeightRequest = MainGrid.Height;
			VideoPlayerView.WidthRequest = MainGrid.WidthRequest;
			VideoPlayerView.VerticalOptions = LayoutOptions.FillAndExpand;

			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;





			HiddenView.TranslationY =metrics.Height;
		}

		public async void   AnimateHiddenViewUp()
		{
			if (HiddenViewStatus != HiddenViewState.Visible)
			{

				var metrics = DeviceDisplay.ScreenMetrics;
				await HiddenView.TranslateTo(0, 0, 2000, Easing.SpringOut);
				HiddenView.FeedView.ScrollTo(HiddenView.FeedView.SelectedItem, ScrollToPosition.Start, true);
				 

				HiddenViewStatus = HiddenViewState.Visible;
			}
		}

		public  async void AnimateHiddenViewDown()
		{
			if (HiddenViewStatus != HiddenViewState.Collapsed)
			{
				var metrics = DeviceDisplay.ScreenMetrics;
				await HiddenView.TranslateTo(0, metrics.Height, 2000, Easing.SpringOut);
				ViewModel?.StartVideo();
				HiddenViewStatus = HiddenViewState.Collapsed;
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			

			this.BindingContext = null;

			this.ViewModel = null;

			



		}

		public void ResetEvents()
        {
            VideoCompleted = delegate { };
        }

		public void FeedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (Navigation.ModalStack.Count == 0)
			{ 
		    ViewModel.SharedData.NextItem = ViewModel.SharedData.GetNextItem();
			AnimateHiddenViewDown();
			}

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
			 

			

		}

		public void StopAnimatingTitles()
		{
		 
			TitleView.Opacity = 0;
			NextShowView.Opacity = 0;

		}


 

		public async Task ShowAnimatedViews()
		{
	 
			while (true)
			{

				ViewModel.SharedData.TitleAnimationState  = TitleAnimationStatus.Playing;

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

	 

	
 
		private void TitleView_Tapped(object sender, EventArgs e)
		{

			 AnimateHiddenViewUp();
		}


		private void NextVideoView_Tapped(object sender, EventArgs e)
		{
			OpenHiddenView();
			//
			//		ViewModel.VideoPage_VideoCompleted();
			//	ViewModel.StartVideo();


		}

 

		private void OpenHiddenView()
		{
			VideoPlayerView.Stop();

			AnimateHiddenViewUp();


		}

 
	}
}
