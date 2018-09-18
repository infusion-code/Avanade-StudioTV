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
					StartVideo();
					selectedItem = null;
					OnPropertyChanged("SelectedItem");
               
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

		#region WeatherData

		private string _city;

		public string City
		{
			get => _city;
			set
			{
				if (_city != value)
				{
					_city = value;
					OnPropertyChanged("City");
				}
			}
		}

		private DayWeatherModel _today;

		public DayWeatherModel Today
		{
			get => _today;
			set
			{
				if (_today != value)
				{
					_today = value;
					OnPropertyChanged("Today");
				}
			}
		}

		private DayWeatherModel _tommorrow;

		public DayWeatherModel Tommorrow
		{
			get => _tommorrow;
			set
			{
				if (_tommorrow != value)
				{
					_tommorrow = value;
					OnPropertyChanged("Tommmorrow");
				}
			}
		}


		private DayWeatherModel _Day3;

		public DayWeatherModel Day3
		{
			get => _Day3;
			set
			{
				if (_Day3 != value)
				{
					_Day3 = value;
					OnPropertyChanged("Day3");
				}
			}
		}


		private DayWeatherModel _Day4;

		public DayWeatherModel Day4
		{
			get => _Day4;
			set
			{
				if (_Day4 != value)
				{
					_Day4 = value;
					OnPropertyChanged("Day4");
				}
			}
		}


		private DayWeatherModel _Day5;

		public DayWeatherModel Day5
		{
			get => _Day5;
			set
			{
				if (_Day5 != value)
				{
					_Day5 = value;
					OnPropertyChanged("Day5");
				}
			}
		}

		public void SetupWeather()
		{
			if (App.DataManager.WeatherForecast?.forecast?.forecastday?.Count > 0)
			{
				City = App.DataManager.WeatherForecast?.location?.name.ToUpper();


				//Today
					Today = new DayWeatherModel();
					Today.Day = DateTime.Now.DayOfWeek.ToString();
			
					Today.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.current?.temp_f.ToString("N0")) ;
				   // Add spaces between string to match design formatting
				    Today.MaxTemp = Today.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Today.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[0]?.day?.mintemp_f.ToString("N0"));
					Today.MinTemp = Today.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Today.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.current?.temp_c.ToString("N0")) ;
				//	Today.MaxTempC = Today.MaxTempC.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Today.type = GetWeatherType(App.DataManager.WeatherForecast?.current.condition);
				//Tommorrow
					Tommorrow = new DayWeatherModel();
					Tommorrow.Day = DateTime.Now.DayOfWeek.ToString();
					Tommorrow.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.maxtemp_f.ToString("N0"));
					Tommorrow.MaxTemp = Tommorrow.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');

				   Tommorrow.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.mintemp_f.ToString("N0"));
					Tommorrow.MinTemp = Tommorrow.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Tommorrow.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.maxtemp_c.ToString("N0"));
					//Tommorrow.MaxTempC = Tommorrow.MaxTempC.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Tommorrow.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[1].day?.condition);
             //Day 3
					Day3 = new DayWeatherModel();
					Day3.Day = DateTime.Now.DayOfWeek.ToString();
					Day3.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[2]?.day?.maxtemp_f.ToString("N0"));
					Day3.MaxTemp = Day3.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day3.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[2]?.day?.mintemp_f.ToString("N0"));
					Day3.MinTemp = Day3.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				    Day3.MaxTempC = string.Format("{0:0}°C",  App.DataManager.WeatherForecast?.forecast?.forecastday[2].day?.maxtemp_c.ToString("N0"));
					//Day3.MaxTempC = Day3.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day3.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[2].day?.condition);
             //Day4
					Day4 = new DayWeatherModel();
					Day4.Day = DateTime.Now.DayOfWeek.ToString();
					Day4.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[3]?.day?.maxtemp_f.ToString("N0"));
					Day4.MaxTemp = Day4.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day4.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[3]?.day?.mintemp_f.ToString("N0"));
					Day4.MinTemp = Day4.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day4.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[3].day?.maxtemp_c.ToString("N0"));
				//	Day4.MaxTempC = Day4.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day4.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[3].day?.condition);
             //Day5
					Day5 = new DayWeatherModel();
					Day5.Day = DateTime.Now.DayOfWeek.ToString();
					Day5.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[4]?.day?.maxtemp_f.ToString("N0"));
					Day5.MaxTemp = Day5.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day5.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[4]?.day?.mintemp_f.ToString("N0"));
					Day5.MinTemp = Day5.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day5.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[4].day?.maxtemp_c.ToString("N0"));
					//Day5.MaxTempC = Day5.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
					Day5.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[4].day?.condition);
			}

		}

		private WeatherType GetWeatherType(Condition2 condition)
		{
			/*Note for this app we only have 3 weather types : Sunny, Cloudy, or Story(anything else) - this is a high level status view and  not a complete app:
				*1000 = Sunny
				* 1003, 1006, 1009 = Cloudy (partly cloudy, overcast etc)
				* Rest is catagorized as Storm */
			if (condition?.code == 1000) return WeatherType.Sunny;
			if ((condition?.code == 1006) || (condition?.code == 1009) || (condition?.code == 1003) || (condition?.code == 1063)
				|| (condition?.code == 1030) || (condition?.code == 1087)) return WeatherType.Cloudy;
			else return WeatherType.Storm;

		}

		private WeatherType GetWeatherType(Models.Condition condition)
		{
			/*Note for this app we only have 3 weather types : Sunny, Cloudy, or Story(anything else) - this is a high level status view and  not a complete app:
				*1000 = Sunny
				* 1003, 1006, 1009 = Cloudy (partly cloudy, overcast etc)
				* Rest is catagorized as Storm */
			if (condition?.code == 1000) return WeatherType.Sunny;
			if ((condition?.code == 1006) || (condition?.code == 1009) || (condition?.code == 1003) || (condition?.code == 1063)
				|| (condition?.code == 1030) || (condition?.code == 1087)) return WeatherType.Cloudy;
			else return WeatherType.Storm;

		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

        public FullScreenListViewViewModel(INavigation navigation)
        {

			//this.GetNewsFeedAsync();
			Navigation = navigation;
		
			openSettingsPage = new Command(OnOpenSettingsPage);

			

			SetupWeather();



		}

		void OnOpenSettingsPage( )
		{
			var settings = new SettingsPage( this.Navigation);
			this.Navigation.PushModalAsync(settings, true);
		}

		public async void GetNewsFeedAsync()
		{

			var result = App.DataManager.GetDataFromNetwork();
			await result;

			List<Item> list = App.DataManager.CurrentPlaylist;

			CurrentChannel = App.DataManager.CurrentChannel;

			if (list != null)
			{
				FeedList?.Clear();
				FeedList = new ObservableCollection<Item>(list);

				this.SelectedItem = FeedList[0];


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
