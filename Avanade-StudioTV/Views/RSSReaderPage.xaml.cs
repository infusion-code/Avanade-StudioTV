using AvanadeStudioTV.ViewModels;
using Xamarin.Forms;

namespace AvanadeStudioTV.Views
{
    public partial class RSSReaderPage : ContentPage
    {

        RSSFeedViewModel RSSFeedViewModelObject;

        public RSSReaderPage(MasterPage Master)
        {
            InitializeComponent();

            RSSFeedViewModelObject = new RSSFeedViewModel(Navigation, Master);


            Title = "Avanade Studio TV";
            BindingContext = RSSFeedViewModelObject;

            this.IsBusy = true;
        }


    }
}
