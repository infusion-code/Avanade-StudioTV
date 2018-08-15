using System;
using System.Collections.Generic;
using System.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer;

using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class VideoPage : ContentPage
    {
        public event VideoCompletedHandler VideoCompleted;
        public delegate void VideoCompletedHandler();
  
        public string Source { get; set; }

        public VideoPage(string source)
        {
            Source = source;
           
            InitializeComponent();

            VideoWebView.OnContentLoaded += VideoWebView_OnContentLoaded;

            VideoWebView.AddLocalCallback("RaiseEndedEvent", LoadNextVideo);
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


        }
      
        public  async void PlayVideo(string url)
        {
            Source = url;
            String pv = "PlayVideo('" + Source + "');";
            Device.BeginInvokeOnMainThread(() =>
            {
                 VideoWebView.InjectJavascriptAsync(pv);
              
                

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
            
        }
    }
}
