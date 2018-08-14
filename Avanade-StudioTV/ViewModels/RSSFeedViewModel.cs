using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Network;
using Xamarin.Forms;

namespace AvanadeStudioTV.ViewModels
{
    public class RSSFeedViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Item> FeedList
        {
            get => feedList;
            set
            {
                if (feedList != value)
                {
                    feedList = value;
                    OnPropertyChanged("FeedList");
                }

            }
        }

        private Item selectedItem = null;
        private INavigation Navigation;
        public Item SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                    OpenWebPage();
                }
            }
        }
        ObservableCollection<Item> feedList = null;
        public event PropertyChangedEventHandler PropertyChanged;

        public RSSFeedViewModel(INavigation navigation)
        {
            this.GetNewsFeedAsync();
            Navigation = navigation;
        }

        public async void GetNewsFeedAsync()
        {
            NetworkManager manager = NetworkManager.Instance;
            List<Item> list = await manager.GetSyncFeedAsync();
            FeedList = new ObservableCollection<Item>(list);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenWebPage()
        {
            //TODO 
            //WebPage page = new WebPage(selectedItem.guid);
           // Navigation.PushAsync(page);
        }
    }
}
