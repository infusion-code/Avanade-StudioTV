using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvanadeStudioTV.Views;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Network;
using Xamarin.Forms;
using System;
using System.Windows.Input;

namespace AvanadeStudioTV.ViewModels
{
    public class RSSFeedViewModel : INotifyPropertyChanged
    {
		ICommand openSettingsPage;

		public ICommand OpenSettingsPage
		{
			get { return openSettingsPage; }
		}
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

		private INavigation Navigation;

		private Item selectedItem = null;
     
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
			openSettingsPage = new Command(OnOpenSettingsPage);
		}

		void OnOpenSettingsPage( )
		{
			var settings = new SettingsPage(this.Master, this.Navigation);
			this.Navigation.PushModalAsync(settings);
		}

		public async void GetNewsFeedAsync()
        {
            NetworkManager manager = NetworkManager.Instance;
            List<Item> list = await manager.GetSyncFeedAsync();
            FeedList = new ObservableCollection<Item>(list);

			//start first video
			this.SelectedItem = FeedList[0];
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
