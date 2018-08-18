using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Realms;


using AvanadeStudioTV.Models;
using AvanadeStudioTV.Database;
using System.Runtime.Serialization;
using System.Linq;

namespace AvanadeStudioTV.Network
{
    public class NetworkManager
    {
        public static NetworkManager network_manager = new NetworkManager();
        private NetworkManager()
        {
			 

			 
		
		}
		
        public static NetworkManager Instance
        {
            get
            {
                return network_manager;
            }
        }

        public async Task<List<Item>> GetSyncFeedAsync()
        {
            if (this.IsConnected())
            {
				var uri =  new Uri(GetUrl());
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(uri);
                String response_string = await response.Content.ReadAsStringAsync();
                FeedItemParser parser = new FeedItemParser();
                // List<FeedItem> list = await Task.Run(() => parser.ParseFeed(response_string));
                List<Item> list = await Task.Run(() => parser.ParseFeed(response_string));
                return list;
            }
            return null;
        }

		private  string GetUrl()
		{
			var realm = Realm.GetInstance();
			var feed =  realm.All<RSSFeedData>().FirstOrDefault();
			return feed.url;
		}

		public bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }
            return false;
        }
    }
   }

