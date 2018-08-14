using AvanadeStudioTV.ViewModels;
using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class RSSReaderPage : ContentPage
    {
        
            RSSFeedViewModel RSSFeedViewModelObject;

            public RSSReaderPage()
            {
                InitializeComponent();

                RSSFeedViewModelObject = new RSSFeedViewModel(Navigation);


                Title = "RSS Feeds";
                BindingContext = RSSFeedViewModelObject;
            }
         
    }
}
