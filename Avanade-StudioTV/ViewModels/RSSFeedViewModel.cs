using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvanadeStudioTV.Views;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Network;
using Xamarin.Forms;
using System;

namespace AvanadeStudioTV.ViewModels
{
    public class RSSFeedViewModel : INotifyPropertyChanged
    {
       
        public MasterPage Master { get; set; }

        public List<string> Playlist { get; set; }

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
                   OpenVideoPage();
                }
            }
        }
        ObservableCollection<Item> feedList = null;
        public event PropertyChangedEventHandler PropertyChanged;

        public RSSFeedViewModel(INavigation navigation, MasterPage master)
        {
            this.Master = master;
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

        private  void OpenVideoPage()
        {
            this.Master.videoPage.ResetEvents();

            this.Master.videoPage.VideoCompleted += VideoPage_VideoCompleted;

			this.Master.videoPage.ViewModel.SelectedItem = selectedItem;

            this.Master.videoPage.PlayVideo(selectedItem.Enclosure.Url);
            this.Master.videoPage.ForceLayout();


        }

        private void VideoPage_VideoCompleted()
        {
            var index = FeedList.IndexOf(SelectedItem) ;
            this.SelectedItem = FeedList[index +1];
            this.Master.ReaderPage.FeedView.ScrollTo(SelectedItem,ScrollToPosition.MakeVisible,true);
        }
    }
}
