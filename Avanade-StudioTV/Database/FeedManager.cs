using AvanadeStudioTV.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Realms;
using System.Linq;
using AvanadeStudioTV.Network;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Timers;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AvanadeStudioTV.Database
{

	public class FeedManager : INotifyPropertyChanged
	{


		#region App Constants

		public  double INTERSTITAL_SCREEN_DISPLAY_INTERVAL = 5000;

		public const string MSDN_CHANNEL9_IMAGE_URL = "https://sec.ch9.ms/content/feedimage.png";

		public const string MIXED_CHANNEL_TITLE = "Channel 9 Video Mix";

		public const double CHECK_WEATHER_INTERVAL = 60 * 60 * 3000; // 5000; //60 * 60 * 3000; //3 Hour interval in milliseconds to call weather API while app is running

		//Using a free weather api account from https://api.apixu.com/
		//example call:
		//https://api.apixu.com/v1/forecast.json?key=25785797d7664c8582201255181609"

		public const string WEATHER_API_KEY = "25785797d7664c8582201255181609";
 
		public const string WEATHER_API_FORCAST_URL = "https://api.apixu.com/v1/forecast.json?key=";

		#endregion
		
		/// <summary>
		/// Boolean to determine whether to show a set of shows from all active feeds (each feed represents a set of shows for a particular microsoft channel 9  show) 
		/// </summary>
		public bool IsMixedFeed { get; set; }

		public event EventHandler NetworkDataLoaded;

		public event EventHandler SelectedItemChanged;


		public List<RSSFeedViewData> AllFeeds { get; set; }
		public Channel CurrentChannel { get; set; }
      public Item CurrentItem { get; set; }

		public List<Item> CurrentPlaylist { get; set; }

		public int CurrentPlaylistIndex { get; set; }

		#region FeedList

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
					SelectedItemChanged?.Invoke(this, EventArgs.Empty);

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

		#endregion

		public bool? IsFullScreenView { get; set; }

		public NetworkManager NetworkService { get; set; }

		public WeatherModel WeatherForecast { get; set; }

		private Timer WeatherTimer;

		public Realm realm { get; set; }

		//for weather location
		public string ZipCode { get; set; }

		public static FeedManager feed_manager = new FeedManager();

		public static FeedManager Instance
		{
			get
			{
				return feed_manager;
			}
		}

		public const string DEFAULTZIP = "77007"; //Houston

		public FeedManager()
		{
			SetupDatabase();


			
			SetupNetworkService();
		

			if (Application.Current.Properties.ContainsKey("IsFullScreenView"))
				IsFullScreenView = Application.Current.Properties["IsFullScreenView"] as bool?;
			if (IsFullScreenView == null)
			{
				if ((Device.Idiom == TargetIdiom.Desktop) || (Device.Idiom == TargetIdiom.TV))
				{
					IsFullScreenView = true;
					Application.Current.Properties["IsFullScreenView"] = IsFullScreenView;
				}
				else
				{
					IsFullScreenView = false;
					Application.Current.Properties["IsFullScreenView"] = IsFullScreenView;
				}
			}

			if (Application.Current.Properties.ContainsKey("ZipCode"))
				ZipCode = Application.Current.Properties["ZipCode"] as string;
			else ZipCode = DEFAULTZIP;

			 
			Task task = Task.Run(async () => await StartWeatherDataRetreivalAsync());


		}

		private async Task StartWeatherDataRetreivalAsync()
       {
			await GetWeatherForcastAsync();
			WeatherTimer = new System.Timers.Timer(CHECK_WEATHER_INTERVAL);//in milliseconds
			WeatherTimer.Elapsed += new ElapsedEventHandler( OnGetWeatherDataEvent);
			WeatherTimer.Start();


		}

		private  async void OnGetWeatherDataEvent(object source, ElapsedEventArgs e)
		{
			await GetWeatherForcastAsync();

		}

		private void SetupDatabase()
		{
			AllFeeds = new List<RSSFeedViewData>();
			 
			CurrentChannel = new Channel();
			CurrentItem = new Item();
			try
			{

				realm = Realm.GetInstance();
			}
			catch (Exception ex)
			{
				var x = ex;
			}

			var RssFeeds = realm?.All<RSSFeedData>().ToList<RSSFeedData>();
			if (RssFeeds.Count < 1) InitalizeDataOnFirstUse();
			else
			{

				//map Realm object to Non Realm object
				foreach (RSSFeedData r in RssFeeds)
				{
					RSSFeedViewData n = new RSSFeedViewData();
					n.ChannelName = r.ChannelName;
					n.Desc = r.Desc;
					n.url = r.url;
					n.imageUrl = r.imageUrl;
					n.isActiveFeed = r.isActiveFeed;

					AllFeeds.Add(n);

					 
				}
			}
		}

		public void SaveFullScreenMode()
		{
			Application.Current.Properties["IsFullScreenView"] = IsFullScreenView;
			Application.Current.SavePropertiesAsync();
	   }


		public void SaveZipCode(string Zip)
		{
			ZipCode = Zip;
			Application.Current.Properties["ZipCode"] = Zip;
			Application.Current.SavePropertiesAsync();
		}

		public async Task <bool> GetWeatherForcastAsync()
		{
			//5 day
			var url = WEATHER_API_FORCAST_URL + WEATHER_API_KEY +"&q=" +ZipCode + "&days=5";

			var model = await NetworkService.GetWeatherForcastAsync(url);


			if (model?.forecast?.forecastday?.Count > 0 )
			{
				WeatherForecast = model;
				 
	              MessagingCenter.Send("obj", "WeatherUpdated");
				 
				return true;

			}
			else return false;
		}

		#region Data Feed Methods

		public Item GetNextItem()
		{
			if (FeedList.Count > 0)
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

			return null;
		}

		public async Task<bool> GetDataFromNetwork()
		{
			

			CurrentPlaylist = new List<Item>();
			CurrentPlaylist.Clear();
			var Channels = realm.All<RSSFeedData>().Where(r => r.isActiveFeed == true);
			foreach (var singleChannel in Channels)
			{
				var list = await NetworkService.GetSyncFeedAsync(singleChannel.url);
				var channelList= ScrubPlaylist(list);
				if (channelList?.Count > 0)
				{
					CurrentPlaylist.AddRange(channelList);
					CurrentChannel = NetworkService.channel; 
				}
			
			}
			
			var temp = CurrentPlaylist?.OrderByDescending(l => DateTime.Parse(l?.PubDate)).ToList();
			var comparer = new ItemComparer();
			CurrentPlaylist = CurrentPlaylist?.Distinct(comparer).ToList();

			if (Channels.Count() > 1)
			{
				IsMixedFeed = true;
				//Set Channel to a custom mix channel
				var c = new Channel
				{
					Title = MIXED_CHANNEL_TITLE,
					Image = new Models.Image
					{
						Url = MSDN_CHANNEL9_IMAGE_URL
					},

				};
				CurrentChannel = c;
			}
			else IsMixedFeed = false;

			//Set Feedlist and first video in collection for all views
			FeedList?.Clear();
			NextItem = null;
			FeedList = new ObservableCollection<Item>(CurrentPlaylist);
			//Whenever we get data from network reset the feed to start at first item
			SelectedItem = FeedList[0]; 
			NextItem = GetNextItem();



			NetworkDataLoaded?.Invoke(this, EventArgs.Empty);
			return true;
		}

     

		private List<Item> ScrubPlaylist(List<Item> list)
		{
			if (list != null)
			{
		
				var x = list.RemoveAll(i => i.Enclosure == null);
				var y = list.RemoveAll(i => i.Enclosure?.Url == String.Empty);

				foreach (Item i in list)
				{

					var t = new Thumbnail();

					t = i.Thumbnail.Find(s => s.Width == "512");
					if (t?.Url != String.Empty)
					{
						i.Thumbnail.Clear();
						i.Thumbnail.Add(t);
					}

					//set parent channel properties
					i.ChannelImageUrl = NetworkService.channel.Image?.Url  ?? NetworkService.channel.Thumbnail[0].Url;
					i.ChannelTitle = NetworkService.channel.Title;
					i.FormattedChannelTitle = FormatTitle(NetworkService.channel.Title);
				}
				return list; 
			}
			return null;
		}

		public string FormatTitle(string value )
		{
			if (  value != String.Empty)
			{
				var s = ( value).ToUpper();
				return s.Aggregate(string.Empty, (c, i) => c + i + ' ');
			}

			return value;
		}

		#endregion

		public async Task<bool> ValidateChannel9FeedUrl(string url)
		{
			var feedItem = await NetworkService.GetSyncFeedAsync(url);
			if (feedItem?.Count > 0)
				return true;
			else return false;
		}



		private void SetupNetworkService()
		{
			NetworkService = NetworkManager.Instance;
		}


		private void InitalizeDataOnFirstUse()
		{

			//iOS Realm Path: /Users/ahmedkhan/Library/Developer/CoreSimulator/Devices/3B877EAC-84BB-4615-823A-8C9BAD0F6DDA/data/Containers/Data/Application/6361196D-D732-4D73-8790-9F4F77F7E873/Documents

			//C:\Users\ahmed.c.khan\AppData\Local\Packages\4851a6aa-f693-4d96-bc70-404b1b69937d_5zacrfw33hrb4\LocalState\default.realm
			System.Diagnostics.Debug.WriteLine($"Realm is located at: {realm.Config.DatabasePath}");

			var RssFeeds = realm.All<RSSFeedData>();

			if (RssFeeds.AsRealmCollection<RSSFeedData>().Count < 1)
			{
				//setup default channel 9 feed if first time launching app:
				var DefaultFeed = new RSSFeedData()
				{
					ChannelName = "Xamarin Show",
					Desc = "Xamarin TV RSS Feed",
					url = "https://s.ch9.ms/Shows/XamarinShow/feed/mp4",
					imageUrl = "https://f.ch9.ms/thumbnail/64fd4835-d6d3-4004-89ea-3f31b43b5dcf.png",

					LastModified = new DateTimeOffset(DateTime.Now),
					isActiveFeed = true

				};

				realm.Write(() =>
				{
					realm.Add<RSSFeedData>(DefaultFeed);
				});

				/* Other Show RSS links:
				 "https://s.ch9.ms/Shows/XamarinShow/feed/mp4";
				 //"https://s.ch9.ms/Shows/This+Week+On+Channel+9/feed/mp4";
				 // "https://s.ch9.ms/Shows/XamarinShow/feed/mp4high"; 
				 //"https://s.ch9.ms/Shows/OEMTV/feed"; 
				 //"https://s.ch9.ms/Feeds/RSS";  
				 //https://s.ch9.ms/Shows/OEMTV/feed

			visual studio toolbox
			https://s.ch9.ms/Shows/Visual-Studio-Toolbox/feed/mp4

			AI Show
			https://s.ch9.ms/Shows/AI-Show/feed
				 */


			}




		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
