using System;
using System.Collections.Generic;
using Octane.Xamarin.Forms.VideoPlayer;

using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class VideoPage : ContentPage
    {
        public string Source { get; set; }

        public VideoPage(string source)
        {
            Source = source;
             

             
            InitializeComponent();
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
            if (Source != "init")
            {

                VideoWebView.Source = Source;


            }
        }

        public void PlayVideo(string url)
        {
            Source = url;
         //   LaunchImage.IsVisible = false;
            VideoWebView.Source = Source;
            


             
             
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
