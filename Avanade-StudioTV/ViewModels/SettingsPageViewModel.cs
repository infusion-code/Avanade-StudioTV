using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvanadeStudioTV.Views;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Network;
using Xamarin.Forms;
using System;
using System.Windows.Input;
using AvanadeStudioTV.Database;
using Realms;
using System.Linq;

namespace AvanadeStudioTV.ViewModels
{
    public class SettingsPageViewModel: INotifyPropertyChanged
    {

		Realm db;
		


		ICommand closeSettingsPage;

		public ICommand CloseSettingsPage
		{
			get { return closeSettingsPage; }
		}

		ICommand addFeed;

		public ICommand AddFeed
		{
			get { return addFeed; }
		}

		public MasterPage Master { get; set; }

        public List<string> Playlist { get; set; }

		ObservableCollection<RSSFeedViewData> feedList;
		public ObservableCollection<RSSFeedViewData> FeedList
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



		private RSSFeedViewData selectedItem = null;

		public RSSFeedViewData SelectedItem
		{
			get => selectedItem;
			set
			{
				if (selectedItem != value)
				{
					selectedItem = value;
					OnPropertyChanged("SelectedItem");
					ValidateFeed();
				}
			}
		}

		private RSSFeedData newFeed = null;
		 
		public RSSFeedData NewFeed
		{
			get => newFeed;
			set
			{
				if (newFeed != value)
				{
					newFeed = value;
					OnPropertyChanged("NewFeed");
					ValidateFeed();
				}
			}
		}

		private void ValidateFeed()
		{
	 
		}

	 
        private INavigation Navigation;

        
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsPageViewModel(INavigation navigation, MasterPage master)
        {
			var realm = Realm.GetInstance();

			var RssFeeds = realm.All<RSSFeedData>().ToList<RSSFeedData>();

			this.FeedList = new ObservableCollection<RSSFeedViewData>();

			//map Realm object to Non Realm object
			foreach (RSSFeedData r in RssFeeds)
			{
				RSSFeedViewData n = new RSSFeedViewData();
				n.ChannelName = r.ChannelName;
				n.Desc = r.Desc;
				n.url = r.url;
				n.imageUrl = r.imageUrl;

				this.FeedList.Add(n);
			}

 
 
			this.Master = master;
            this.GetNewsFeedAsync();
            Navigation = navigation;

			closeSettingsPage = new Command(OnCloseSettingsPage);
			addFeed = new Command(OnAddFeed);

		}

		private void OnAddFeed(object obj)
		{
			ValidateFeed();
		}

		private void OnCloseSettingsPage(object obj)
		{
			SaveUpdates();
			this.Navigation.PopModalAsync();
		}

		private void SaveUpdates()
		{
			 
		}

		public async void GetNewsFeedAsync()
        {
            NetworkManager manager = NetworkManager.Instance;
           List<Item> list = await manager.GetSyncFeedAsync();
            
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

 
 
    }
}
