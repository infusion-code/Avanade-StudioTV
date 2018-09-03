using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class MasterPage : MasterDetailPage
    {
        public RSSReaderPage ReaderPage;
        public VideoPage videoPage { get; set; }
        public NavigationPage Nav { get; set; }

        public MasterPage()
        {
         

            InitializeComponent();
            ReaderPage = new RSSReaderPage(this);
            videoPage = new VideoPage("init");

            Nav = new NavigationPage(videoPage);
            Nav.BackgroundColor = Color.Transparent;
            Nav.BarBackgroundColor = Color.Transparent;
            NavigationPage.SetHasNavigationBar(videoPage, false);

            this.Master = ReaderPage;
            this.Detail = Nav;

        }

    }
}
