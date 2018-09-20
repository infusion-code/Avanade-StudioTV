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

		private FeedManager _SharedData { get; set; }
		public FeedManager SharedData
		{
			get => _SharedData;
			set
			{
				if (_SharedData != value)
				{
					_SharedData = value;
					OnPropertyChanged("SharedData");

				}
			}
		}



		private INavigation Navigation;
		private FullScreenVideoPage VideoPage;



		public event PropertyChangedEventHandler PropertyChanged;

        public FullScreenVideoViewModel(INavigation navigation, FullScreenVideoPage videopage)
        {

			//this.GetNewsFeedAsync();
			Navigation = navigation;
			VideoPage = videopage;

			MessagingCenter.Subscribe<string>(this, "LaunchVideo", async (obj) =>
			{
				StartVideo();
			});

			SharedData = App.DataManager;
		}



		public async void GetNewsFeedFromNetworkAsync()
		{
			var result =  SharedData.GetDataFromNetwork();
			await result;
 

			if (SharedData.FeedList.Count == 0) ShowErrorMessage();

			StartVideo();
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
			if (SharedData?.SelectedItem != null)
			{
				if (SharedData?.SelectedItem?.Enclosure?.Url != String.Empty)
				{
					SharedData.SelectedItem.BackgroundColor = "#009999";
					VideoPage.ResetEvents();
					VideoPage.VideoCompleted += VideoPage_VideoCompleted;
					VideoPage.PlayVideo(SharedData.SelectedItem.Enclosure.Url);
					VideoPage.ForceLayout();
					VideoPage.ShowAnimations = true;


				}

				else VideoPage_VideoCompleted(); 
			}


		}
		public void VideoPage_VideoCompleted()
		{
			VideoPage.StopAnimatingTitles();
			VideoPage.ShowAnimations = false;
			SharedData.SelectedItem = SharedData.GetNextItem();
			SharedData.NextItem = SharedData.GetNextItem();

			StartVideo();

		//	if (Navigation.ModalStack.Count == 0)
		//	Navigation.PushModalAsync(new FullScreenListPage(true));

		}




	}
}
