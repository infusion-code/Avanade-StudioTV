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
                   OpenVideoPage();
                }
            }
        }
     
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
			var settings = new SettingsPage(this.Master, this.Navigation, this);
			this.Navigation.PushModalAsync(settings);
		}

		public async void GetNewsFeedAsync()
		{
			var connected = await App.DataManager.GetDataFromNetwork();
			List<Item> list = App.DataManager.CurrentPlaylist;

			CurrentChannel = App.DataManager.CurrentChannel;

			if (list != null)
			{
				
				FeedList = new ObservableCollection<Item>(list);

				this.SelectedItem = FeedList[0];
			

			}

			else ShowErrorMessage();
		}

 

		private void ShowErrorMessage()
		{
			this.Master.DisplayAlert("Error", "Could not connect to Feed Data", "OK");
		}

		protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private  void OpenVideoPage()
        {
			this.SelectedItem.BackgroundColor = "#009999";
            this.Master.videoPage.ResetEvents();
            this.Master.videoPage.VideoCompleted += VideoPage_VideoCompleted;
			this.Master.videoPage.ViewModel.SelectedItem = selectedItem;
            this.Master.videoPage.PlayVideo(selectedItem.Enclosure.Url);
            this.Master.videoPage.ForceLayout();


        }


		private void VideoPage_VideoCompleted()
        {
            var index = FeedList.IndexOf(SelectedItem) ;
			if (FeedList.ElementAtOrDefault(index + 1) != null)
			{
				this.SelectedItem = FeedList[index + 1];
			}
			//Loop playlist 
			//TODO need implement multiple playlists here
			else
			{
				this.SelectedItem = FeedList[0];
			}
			this.Master.ReaderPage.FeedView.ScrollTo(SelectedItem,ScrollToPosition.MakeVisible,true);
        }
    }
}
