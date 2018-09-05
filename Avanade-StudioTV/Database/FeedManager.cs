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


		 

		public const string MSDN_CHANNEL9_IMAGE_URL = "https://sec.ch9.ms/content/feedimage.png";

		public const string MIXED_CHANNEL_TITLE = "Channel 9 Video Mix";

		/// <summary>
		/// Boolean to determine whether to show a set of shows from all active feeds (channels) 
		/// </summary>
		public bool IsMixedFeed { get; set; }

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

		public async Task<bool> GetDataFromNetwork()
		{
			CurrentPlaylist = new List<Item>();
			var Channels = realm.All<RSSFeedData>().Where(r => r.isActiveFeed == true);
			foreach (var singleChannel in Channels)
			{
				var channelFeed = ScrubPlaylist(await NetworkService.GetSyncFeedAsync(singleChannel.url));
				CurrentPlaylist.AddRange(channelFeed);
				CurrentChannel = NetworkService.channel;
			
			}
			
			CurrentPlaylist = CurrentPlaylist.OrderByDescending(l => DateTime.Parse(l.PubDate)).ToList();

			if (Channels.Count() > 1)
			{
				IsMixedFeed = true;
				//Set Channel to a custom mix channel
				var c = new Channel
				{
					Title = MIXED_CHANNEL_TITLE,
					Image = new Image
					{
						Url = MSDN_CHANNEL9_IMAGE_URL
					},

				};
				CurrentChannel = c;
			}
			else IsMixedFeed = false;
			return true;
		}

 

		private List<Item> ScrubPlaylist(List<Item> list)
		{
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
				i.ChannelImageUrl = NetworkService.channel.Image.Url;
				i.ChannelTitle = NetworkService.channel.Title;
			}
			return list;
		}

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
	}
}
