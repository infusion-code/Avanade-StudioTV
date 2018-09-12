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
using System.Threading.Tasks;

namespace AvanadeStudioTV.ViewModels
{
    public class SettingsPageViewModel: INotifyPropertyChanged
    {

		Realm db;



		private RSSFeedViewModel RSSViewModel;

		private bool isOriginallyFullScreen;

		private bool _isFullScreen;
		public bool IsFullScreen
		{
			get => _isFullScreen;
			set
			{
				_isFullScreen = value;
				App.DataManager.IsFullScreenView = _isFullScreen;
				App.DataManager.SaveFullScreenMode();
				OnPropertyChanged("IsFullScreen");
			}
		}


		private bool _isChecked;
		public bool IsChecked
		{
			get => _isChecked;
			set
			{
				_isChecked = value;
				TextCheckBox = _isChecked ? "Is Playing" : "Is Not Playing";
				OnPropertyChanged("IsChecked");
			}
		}

		private string _textCheckBox;
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

		ICommand closeSettingsPage;
		public ICommand CloseSettingsPage
		{
			get { return closeSettingsPage; }
		}

	
		 

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
					ValidateFeed(NewFeed.url);
				}
			}
		}

		private RSSFeedViewData newFeed = null;
		 
		public RSSFeedViewData NewFeed
		{
			get => newFeed;
			set
			{
				if (newFeed != value)
				{
					newFeed = value;
					OnPropertyChanged("NewFeed");
					 
				}
			}
		}

		public ICommand addFeedCommand;
		public ICommand  AddFeedCommand
		{
			get
			{
				return addFeedCommand;
			}
		}

		public SettingsPageViewModel(INavigation navigation, bool isFullScreenView)
		{
			this.isOriginallyFullScreen = isFullScreenView;
			this.IsFullScreen = (bool) App.DataManager.IsFullScreenView;

			this.NewFeed = new RSSFeedViewData();
			this.NewFeed.isActiveFeed = true;
			 
			this.GetNewsFeedAsync();
			Navigation = navigation;

			closeSettingsPage = new Command(OnCloseSettingsPage);

			addFeedCommand = new Command(async () => { await AddFeed(); });

		//	OnCheckedChanged = new Command(SetActiveFeed);

		}

		/*private void SetActiveFeed()
		{
			//Remove active feed flag for all other feeds (only 1 feed can be active in current UI)
			if (SelectedItem != null && SelectedItem?.isActiveFeed == true)
			{
			  	FeedList?.Where(c => c.Desc != SelectedItem.Desc).ToList().ForEach(i => i.isActiveFeed = false);
				var x = FeedList;
			}
		} */

		private async Task<bool> AddFeed()
		{
		 var isValid =	await ValidateFeed(NewFeed.url);
			if (isValid)
			{
				NewFeed.DeleteCommand  = new Command<RSSFeedViewData>((FeedItem) => {
					FeedList.Remove(FeedItem);
				});
				this.FeedList.Add(NewFeed);
				NewFeed = new RSSFeedViewData();
				 SaveAsync();
				GetNewsFeedAsync();
				return true;
			}

			else
			{
				await Application.Current.MainPage.DisplayAlert("ERROR READING FEED", "Please Correct Feed URL or Desc, Should be a valid Channel 9 RSS Feed Url with a Show Name in the Description", "OK");
				return false;
			}
		}

	 



		private async Task<bool> ValidateFeed(string url)
		{
			

			var isValid  = await App.DataManager.ValidateChannel9FeedUrl(url);
			if (isValid)
			{ 
			 NewFeed.ChannelName = App.DataManager.NetworkService.channel.Title;
				NewFeed.imageUrl = App.DataManager.NetworkService.channel.Image.Url;
				return true;
			}

			return false;
		}

	 
        private INavigation Navigation;

        
        public event PropertyChangedEventHandler PropertyChanged;



	 

		private void OnCloseSettingsPage(object obj)
		{
		 
			SaveAsync();

			CheckAppLayout();
 
			{
				//Reload video page with new videos
				MessagingCenter.Send("obj", "Update");
			}
		}

		private void CheckAppLayout()
		{
			if (IsFullScreen)
			{
				if (isOriginallyFullScreen)
				{
					this.Navigation.PopModalAsync();
				}
				else
				{
					Application.Current.MainPage = new FullScreenVideoPage();
				
				}
			}

		   else
			{
				if (!isOriginallyFullScreen)
				{
					this.Navigation.PopModalAsync();
				}

				else
				{
					Application.Current.MainPage = new MasterPage();
					
				}

			}
		}

		private async void SaveAsync()
		{
			if (!FeedList.Any(f => f.isActiveFeed)) FeedList[0].isActiveFeed = true;

			App.DataManager.realm.Write(() =>
			{
				App.DataManager.realm.RemoveAll();

				foreach (RSSFeedViewData r in this.FeedList)
				{
					var n = new RSSFeedData();
					n.ChannelName = r.ChannelName;
					n.url = r.url;
					n.imageUrl = r.imageUrl;
					n.isActiveFeed = r.isActiveFeed;
					n.Desc = r.Desc;

					App.DataManager.realm.Add(n);
				}

				App.DataManager.AllFeeds = this.FeedList.ToList<RSSFeedViewData>();
			});

	
		}

		public async void GetNewsFeedAsync()
        {
			var result = await App.DataManager.GetDataFromNetwork();

		   var list =  App.DataManager.CurrentPlaylist;

			var all = new ObservableCollection<RSSFeedViewData>(App.DataManager.AllFeeds);

			foreach (RSSFeedViewData r in all)
			{
				//1. Set checkbox changed command for all feeds in FeedList
				r.MakeActiveFeedCommand = OnCheckedChanged;
					
				//2. Set Delete command for all Feeds in Feedlist
				r.DeleteCommand = new Command<RSSFeedViewData>((FeedItem) => {
					if (FeedList.Count >1)
					{
						FeedList.Remove(FeedItem);
					    if (FeedList.Any(f=>f.isActiveFeed)) SaveAsync();
						else //If none are marked as being active mark the first one so that we keep an active playlist
						{
							FeedList[0].isActiveFeed = true;
							SaveAsync();
						}
					}

					else
					{
						 Application.Current.MainPage.DisplayAlert("Error", "Cannot remove last Feed, Please add another feed first", "OK");

					}
				});
			}

			this.FeedList = all;
		}

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

 
 
    }
}
