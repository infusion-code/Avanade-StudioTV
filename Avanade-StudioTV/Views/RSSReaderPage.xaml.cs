using System;
using AvanadeStudioTV.ViewModels;
using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class RSSReaderPage : ContentPage
    {

        RSSFeedViewModel RSSFeedViewModelObject;
        public ListView FeedView { get; set; }

		public MasterPage master;

        public RSSReaderPage(MasterPage Master)
        {
            InitializeComponent();

            FeedView = this.FeedListView;

            RSSFeedViewModelObject = new RSSFeedViewModel(Navigation, Master);

			master = Master;


            Title = "Avanade Studio TV";
            BindingContext = RSSFeedViewModelObject;

			//Subscibe to insert expenses
			MessagingCenter.Subscribe<string>(this, "Update", (obj) =>
			{
				this.Reload();
			});


		}

		private void Reload()
		{
			

			FeedView = this.FeedListView;

			RSSFeedViewModelObject = new RSSFeedViewModel(Navigation, master);


			Title = "Avanade Studio TV";
			BindingContext = RSSFeedViewModelObject;
			
		}
	}
}
