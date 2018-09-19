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
using Avanade_StudioTV;
using System.Linq;
using System.Threading.Tasks;

namespace AvanadeStudioTV.ViewModels
{
    public class FullScreenVideoViewModel : INotifyPropertyChanged
    {

	

        public List<string> Playlist { get; set; }

	


		ObservableCollection<Item> feedList = null;
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
		private FullScreenVideoPage VideoPage;


		private Channel currentChannel = null;

		public Channel CurrentChannel
		{
			get => currentChannel;
			set
			{
				if (currentChannel != value)
				{
					currentChannel = value;
					OnPropertyChanged("CurrentChannel");
					 
				}
			}
		}

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
                   StartVideo();
                }
            }
        }

		private Item nextItem = null;

		public Item NextItem
		{
			get => nextItem;
			set
			{
				if (nextItem != value)
				{
					nextItem = value;
					OnPropertyChanged("NextItem");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

        public FullScreenVideoViewModel(INavigation navigation, FullScreenVideoPage videopage)
        {

			//this.GetNewsFeedAsync();
			Navigation = navigation;
			VideoPage = videopage;

			MessagingCenter.Subscribe<string>(this, "LaunchVideo", (obj) =>
			{
				FeedList = new ObservableCollection<Item>(App.DataManager.CurrentPlaylist);
				this.SelectedItem = FeedList[App.DataManager.CurrentPlaylistIndex];
				this.NextItem = GetNextItem();
			});


		}



		public async void GetNewsFeedAsync()
		{
			var result =  App.DataManager.GetDataFromNetwork();
			await result;
			List<Item> list = App.DataManager.CurrentPlaylist;

			CurrentChannel = App.DataManager.CurrentChannel;

			App.DataManager.CurrentPlaylistIndex = 0;

			if (list != null)
			{
				
				FeedList = new ObservableCollection<Item>(list);

				this.SelectedItem = FeedList[App.DataManager.CurrentPlaylistIndex];
				this.NextItem = GetNextItem();

			}

			else ShowErrorMessage();
		}

 

		private void ShowErrorMessage()
		{
			Application.Current.MainPage.DisplayAlert("Error", "Could not connect to Feed Data", "OK");
		}

		protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
		public void StartVideo()
		{
			if (selectedItem.Enclosure?.Url != String.Empty)
			{
				this.SelectedItem.BackgroundColor = "#009999";
				VideoPage.ResetEvents();
				VideoPage.VideoCompleted += VideoPage_VideoCompleted;
				VideoPage.PlayVideo(selectedItem.Enclosure.Url);
				VideoPage.ForceLayout();
			}

			else VideoPage_VideoCompleted();


		}
		public void VideoPage_VideoCompleted()
		{

			SelectedItem = GetNextItem();
			NextItem = GetNextItem();

			 
			App.DataManager.CurrentPlaylistIndex = FeedList.IndexOf(SelectedItem);

		}



		private Item  GetNextItem()
		{
			var index = FeedList.IndexOf(SelectedItem);
			App.DataManager.CurrentPlaylistIndex = index;
			if (FeedList.ElementAtOrDefault(App.DataManager.CurrentPlaylistIndex + 1) != null)
			{
				 

				return FeedList[App.DataManager.CurrentPlaylistIndex + 1];
			}
			//Loop playlist 
			//TODO need implement multiple playlists here
			else
			{
			 
				return FeedList[0];
			}

		}
	}
}
