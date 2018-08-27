using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xam.Plugin.WebView;
using Xam.Plugin.WebView.Abstractions;

using FormsVideoLibrary;

using Xamarin.Forms;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.ViewModels;

namespace AvanadeStudioTV.Views
{
    public partial class VideoPage : ContentPage
    {
        public event VideoCompletedHandler VideoCompleted;
        public delegate void VideoCompletedHandler();
		public VideoPageViewModel ViewModel;

		//This setting switches back and forth between using a Native Video Player or Web Based Video Player inUWP:
		//Reason: Because of known issue with Video Player not rendering in UWP.Release config had to add for a native player -
		//issue was fixed so swiching back to Web based video player for all platforms for now:
		public const bool UseNativePlayerOnUWP = true;
		public bool UseWebPlayer;

		public string Source { get; set; }

        public FormsWebView VideoWebView;

        public VideoPage(string source)
        {
            Source = source;
           
            InitializeComponent();

			SetUseNativePlayer();


		   if (UseWebPlayer) {

                VideoPlayerView.IsEnabled = false;
                VideoPlayerView.IsVisible = false;

                VideoWebView = new FormsWebView();
                VideoWebView.Source = "video.html";
                VideoWebView.ContentType = Xam.Plugin.WebView.Abstractions.Enumerations.WebViewContentType.LocalFile;
                VideoStack.Children.Add(VideoWebView);

               VideoWebView.OnContentLoaded += VideoWebView_OnContentLoaded;

               VideoWebView.AddLocalCallback("RaiseEndedEvent", LoadNextVideo);

               }


			ViewModel = new VideoPageViewModel();
			this.BindingContext = ViewModel;


		}

		private void SetUseNativePlayer()
		{
			UseWebPlayer = true;
		  switch (Device.RuntimePlatform)
			{
				case Device.UWP:
					if (UseNativePlayerOnUWP)
						UseWebPlayer = false;
					break;
				case Device.iOS:
					UseWebPlayer = true;
					break;
				case Device.Android:
					UseWebPlayer = true;
					break;


			}
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

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();





			if (!UseWebPlayer)
			{
				VideoPlayerView.HeightRequest = VideoStack.Height;
				VideoPlayerView.WidthRequest = VideoStack.WidthRequest;
				VideoPlayerView.VerticalOptions = LayoutOptions.FillAndExpand;
				VideoPlayerView.Source = VideoSource.FromResource("AvanadeStudioIntro.mp4");
				 
				VideoPlayerView.Play(); 
			}
            


        }
      
        public async void PlayVideo(string url)
        {
            Source = url;
            String pv = "PlayVideo('" + Source + "');";
            Device.BeginInvokeOnMainThread(() =>
            {
				 if (UseWebPlayer)
				{
					VideoWebView.InjectJavascriptAsync(pv);
					VideoWebView.InjectJavascriptAsync("RemoveScrolling();");
				}
				else
				{
					
					VideoPlayerView.Source = VideoSource.FromUri(url);
					VideoPlayerView.Play();
					
					VideoPlayerView.VideoEnded += VideoPlayerView_VideoEnded;
				}
			});
   
             
        }

	 

		private void VideoPlayerView_VideoEnded(object sender, EventArgs e)
        {
            VideoCompleted?.Invoke();
			VideoPlayerView.VideoEnded -= VideoPlayerView_VideoEnded;
			 
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
            
        }
    }
}
