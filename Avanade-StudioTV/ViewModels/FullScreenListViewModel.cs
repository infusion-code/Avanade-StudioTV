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
using System.Threading;

namespace AvanadeStudioTV.ViewModels
{
    public class FullScreenListViewViewModel : INotifyPropertyChanged
    {
		ICommand openSettingsPage;

		public ICommand OpenSettingsPage
		{
			get { return openSettingsPage; }
		}

		public Timer KeepListPageVisibleTimer;
		public const double LISTPAGE_VISIBLE_SCREEN_TIME = 10000; //10 sec
 


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

        public FullScreenListViewViewModel(INavigation navigation)
        {

			//this.GetNewsFeedAsync();
			Navigation = navigation;
		
			openSettingsPage = new Command(OnOpenSettingsPage);

			SetupListView();



		}

		void OnOpenSettingsPage( )
		{
			var settings = new SettingsPage( this.Navigation);
			this.Navigation.PushModalAsync(settings);
		}

		public void SetupListView()
		{
			 
			List<Item> list = App.DataManager.CurrentPlaylist;

			CurrentChannel = App.DataManager.CurrentChannel;

			if (list != null)
			{
				
				FeedList = new ObservableCollection<Item>(list);

				this.SelectedItem = FeedList[0];
				this.NextItem = GetNextItem();

			}

			 
		}

 

		private void ShowErrorMessage()
		{
			Application.Current.MainPage.DisplayAlert("Error", "Could not connect to Feed Data", "OK");
		}

		protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public  void StartVideo()
        {
			//TODO Play video on full screen page from here


		}


	

		private Item  GetNextItem()
		{
			var index = FeedList.IndexOf(SelectedItem);
			if (FeedList.ElementAtOrDefault(index + 1) != null)
			{
				return FeedList[index + 1];
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
