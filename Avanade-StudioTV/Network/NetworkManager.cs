using System;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
 

using AvanadeStudioTV.Models;

namespace AvanadeStudioTV.Network
{
    public class NetworkManager
    {
        public static NetworkManager network_manager = new NetworkManager();
        public static string network_url = "https://s.ch9.ms/Feeds/RSS";
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
                Uri uri = new Uri(network_url);
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

