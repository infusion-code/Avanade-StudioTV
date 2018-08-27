using AvanadeStudioTV.ViewModels;
using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class RSSReaderPage : ContentPage
    {

        RSSFeedViewModel RSSFeedViewModelObject;
        public ListView FeedView { get; set; }

        public RSSReaderPage(MasterPage Master)
        {
            InitializeComponent();

            FeedView = this.FeedListView;

            RSSFeedViewModelObject = new RSSFeedViewModel(Navigation, Master);


            Title = "Avanade Studio TV";
            BindingContext = RSSFeedViewModelObject;

             
        }

	


    }
}
