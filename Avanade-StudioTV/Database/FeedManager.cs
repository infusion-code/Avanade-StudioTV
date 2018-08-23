using AvanadeStudioTV.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Realms;
using System.Linq;
using AvanadeStudioTV.Network;
using System.Threading.Tasks;

namespace AvanadeStudioTV.Database
{

	public class FeedManager
	{
		public RSSFeedViewData CurrentFeed { get; set; }

		public List<RSSFeedViewData> AllFeeds { get; set; }
		public Channel CurrentChannel { get; set; }
		public Item CurrentItem { get; set; }
		public List<Item> CurrentPlaylist { get; set; }

		public NetworkManager NetworkService { get; set; }

		public Realm realm { get; set; }

		public static FeedManager feed_manager = new FeedManager();

		public static FeedManager Instance
		{
			get
			{
				return feed_manager;
			}
		}

		public FeedManager()
		{
			SetupDatabase();
			SetupNetworkService();

		}

		private void SetupDatabase()
		{
			AllFeeds = new List<RSSFeedViewData>();
			CurrentFeed = new RSSFeedViewData();
			CurrentChannel = new Channel();
			CurrentItem = new Item();

			realm = Realm.GetInstance();
			var RssFeeds = realm.All<RSSFeedData>().ToList<RSSFeedData>();
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

					if (n.isActiveFeed) CurrentFeed = n;
				}
			}
		}

		public async Task<bool> GetDataFromNetwork()
		{
			var feed = realm.All<RSSFeedData>().Where(r => r.isActiveFeed == true).FirstOrDefault();
			CurrentPlaylist = await NetworkService.GetSyncFeedAsync(feed.url);
			CurrentChannel = NetworkService.channel;
			return true;
		}

		private void SetupNetworkService()
		{
			NetworkService = NetworkManager.Instance;
		}


		private void InitalizeDataOnFirstUse()
		{



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
				 */


			}


		}
	}
}
