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
using Avanade_StudioTV;

namespace AvanadeStudioTV.ViewModels
{
    public class SettingsPageViewModel: INotifyPropertyChanged
    {

		Realm db;
		


		ICommand closeSettingsPage;

		private bool _isChecked;
		private string _textCheckBox;

		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				_isChecked = value;
				TextCheckBox = _isChecked ? "Is Playing" : "Is Playing";
				OnPropertyChanged("IsChecked");
			}
		}

		public string TextCheckBox
		{
			get => _textCheckBox;
			set
			{
				_textCheckBox = value;
				OnPropertyChanged("TextCheckBox");
			}
		}


		public Command OnCheckedChanged { get; set; }

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
			var result = await App.DataManager.GetDataFromNetwork();

		   var list =  App.DataManager.CurrentPlaylist;

			this.FeedList = new ObservableCollection<RSSFeedViewData>(App.DataManager.AllFeeds);


		}

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

 
 
    }
}
