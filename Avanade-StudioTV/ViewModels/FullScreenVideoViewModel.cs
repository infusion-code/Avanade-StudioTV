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
using System.Timers;

namespace AvanadeStudioTV.ViewModels
{
    public class FullScreenVideoViewModel : INotifyPropertyChanged
    {
		 

 
		public System.Timers.Timer DisplayTimoutTimer;

		ICommand openSettingsPage;
		public ICommand OpenSettingsPage
		{
			get { return openSettingsPage; }
		}

		public Timer KeepListPageVisibleTimer;

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

				_today = value;
				OnPropertyChanged("Today");

			}
		}

		private DayWeatherModel _tommorrow;

		public DayWeatherModel Tommorrow
		{
			get => _tommorrow;
			set
			{

				_tommorrow = value;
				OnPropertyChanged("Tommmorrow");

			}
		}


		private DayWeatherModel _Day3;

		public DayWeatherModel Day3
		{
			get => _Day3;
			set
			{

				_Day3 = value;
				OnPropertyChanged("Day3");

			}
		}


		private DayWeatherModel _Day4;

		public DayWeatherModel Day4
		{
			get => _Day4;
			set
			{

				_Day4 = value;
				OnPropertyChanged("Day4");

			}
		}


		private DayWeatherModel _Day5;

		public DayWeatherModel Day5
		{
			get => _Day5;
			set
			{

				_Day5 = value;
				OnPropertyChanged("Day5");

			}
		}
		#endregion

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


			

			MessagingCenter.Subscribe<string>(this, "WeatherUpdated", (obj) =>
			{

				SetupWeather();

			});

			SharedData = App.DataManager;

			SharedData.NetworkDataLoaded += SharedData_NetworkDataLoaded;


			//ListView 


			openSettingsPage = new Command(OnOpenSettingsPage);

			Today = new DayWeatherModel();
			Today.Day = DateTime.Now.DayOfWeek.ToString();
			Tommorrow = new DayWeatherModel();
			Tommorrow.Day = DateTime.Now.DayOfWeek.ToString();
			Day3 = new DayWeatherModel();
			Day3.Day = DateTime.Now.DayOfWeek.ToString();
			Day4 = new DayWeatherModel();
			Day4.Day = DateTime.Now.DayOfWeek.ToString();
			Day5 = new DayWeatherModel();
			Day5.Day = DateTime.Now.DayOfWeek.ToString();

			 

			SetupWeather();

		}

		private void SharedData_NetworkDataLoaded(object sender, EventArgs e)
		{
			StartVideo();
			
			SharedData.NetworkDataLoaded -= SharedData_NetworkDataLoaded;
		}

		public void SetupWeather()
		{
			if (App.DataManager.WeatherForecast?.forecast?.forecastday?.Count > 0)
			{
				City = App.DataManager.WeatherForecast?.location?.name.ToUpper();


				//Today
				;
				Today.Day = DateTime.Now.DayOfWeek.ToString();
				Today.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.current?.temp_f.ToString("N0"));
				if (Today.MaxTemp == String.Empty) Today.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[0]?.day?.maxtemp_f.ToString("N0"));
				// Add spaces between string to match design formatting
				Today.MaxTemp = Today.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Today.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[0]?.day?.mintemp_f.ToString("N0"));
				Today.MinTemp = Today.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Today.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.current?.temp_c.ToString("N0"));
				//	Today.MaxTempC = Today.MaxTempC.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Today.type = GetWeatherType(App.DataManager.WeatherForecast?.current.condition);
				//Tommorrow
				Tommorrow.Day = DateTime.Now.AddDays(1).DayOfWeek.ToString();
				Tommorrow.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.maxtemp_f.ToString("N0"));
				Tommorrow.MaxTemp = Tommorrow.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');

				Tommorrow.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.mintemp_f.ToString("N0"));
				Tommorrow.MinTemp = Tommorrow.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Tommorrow.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[1]?.day?.maxtemp_c.ToString("N0"));
				//Tommorrow.MaxTempC = Tommorrow.MaxTempC.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Tommorrow.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[1].day?.condition);
				//Day 3
				Day3.Day = DateTime.Now.AddDays(2).DayOfWeek.ToString();
				Day3.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[2]?.day?.maxtemp_f.ToString("N0"));
				Day3.MaxTemp = Day3.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day3.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[2]?.day?.mintemp_f.ToString("N0"));
				Day3.MinTemp = Day3.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day3.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[2].day?.maxtemp_c.ToString("N0"));
				//Day3.MaxTempC = Day3.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day3.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[2].day?.condition);
				//Day4
				Day4.Day = DateTime.Now.AddDays(3).DayOfWeek.ToString();
				Day4.MaxTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[3]?.day?.maxtemp_f.ToString("N0"));
				Day4.MaxTemp = Day4.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day4.MinTemp = string.Format("{0:0}°", App.DataManager.WeatherForecast?.forecast?.forecastday[3]?.day?.mintemp_f.ToString("N0"));
				Day4.MinTemp = Day4.MinTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day4.MaxTempC = string.Format("{0:0}°C", App.DataManager.WeatherForecast?.forecast?.forecastday[3].day?.maxtemp_c.ToString("N0"));
				//	Day4.MaxTempC = Day4.MaxTemp.Aggregate(string.Empty, (c, i) => c + i + ' ');
				Day4.type = GetWeatherType(App.DataManager.WeatherForecast.forecast?.forecastday[3].day?.condition);
				//Day5
				Day5.Day = DateTime.Now.AddDays(4).DayOfWeek.ToString();
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
		//same as above but for other condition object
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

		void OnOpenSettingsPage()
		{
			//remove change
			SharedData.NetworkDataLoaded += SharedData_NetworkDataLoaded;
		
			var settings = new SettingsPage(this.Navigation);
			this.Navigation.PushModalAsync(settings, true);
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
					SharedData.SelectedItem.IsSelected = false;
					VideoPage.ResetEvents();
					VideoPage.VideoCompleted += VideoPage_VideoCompleted;
					VideoPage.PlayVideo(SharedData.SelectedItem.Enclosure.Url);
					VideoPage.ForceLayout();

					if (SharedData.TitleAnimationState == TitleAnimationStatus.NotPlaying)
						Task.Run(async () => await VideoPage.ShowAnimatedViews());



				}

				else VideoPage_VideoCompleted(); 
			}


		}
		public void  VideoPage_VideoCompleted()
		{


			SharedData.SelectedItem = SharedData.GetNextItem();
			
			SharedData.NextItem = SharedData.GetNextItem();
			 

			VideoPage.AnimateHiddenViewUp();

			DisplayTimoutTimer = new System.Timers.Timer();
			DisplayTimoutTimer.Interval = App.DataManager.INTERSTITAL_SCREEN_DISPLAY_INTERVAL; // 10 second  
			DisplayTimoutTimer.Elapsed += DisplayTimoutTimer_Elapsed;
			DisplayTimoutTimer.Start();
		

		}

		private void DisplayTimoutTimer_Elapsed(object sender, ElapsedEventArgs e)
		{


			Device.BeginInvokeOnMainThread(() =>
			{
				DisplayTimoutTimer.Stop();
				DisplayTimoutTimer.Dispose();
				VideoPage.AnimateHiddenViewDown();

				


			});
		}





	}

	public enum HiddenViewState
	{
		Collapsed,
		Visible
	}


}
