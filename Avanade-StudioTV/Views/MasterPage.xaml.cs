using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class MasterPage : MasterDetailPage
    {
        RSSReaderPage ReaderPage;
        public VideoPage videoPage { get; set; }

        public MasterPage()
        {
            InitializeComponent();
            ReaderPage = new RSSReaderPage(this);
            videoPage = new VideoPage("init");
            this.Master = ReaderPage;
            this.Detail = new NavigationPage(videoPage);

        }
    }
}
