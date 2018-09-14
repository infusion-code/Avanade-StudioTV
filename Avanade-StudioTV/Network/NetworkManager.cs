using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Realms;

using AvanadeStudioTV;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Database;
using System.Runtime.Serialization;
using System.Linq;
using Newtonsoft.Json;

namespace AvanadeStudioTV.Network
{
    public class NetworkManager
    {
        public static NetworkManager network_manager = new NetworkManager();

		public Channel channel { get; set; }
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

        public async Task<List<Item>> GetSyncFeedAsync(string url)
        {
            if (this.IsConnected())
            {
				try
				{
					var uri = new Uri(url);
					HttpClient client = new HttpClient();
					HttpResponseMessage response = await client.GetAsync(uri);
					String response_string = await response.Content.ReadAsStringAsync();
					FeedItemParser parser = new FeedItemParser();

					List<Item> list = await Task.Run(() => parser.ParseFeed(response_string));
					channel = parser.Channel9RSSDATA.Channel;
					return list;
				}
				catch (Exception e)
				{
					//TODO add exception handling
					return null;
				}
            }
            return null;
        }

		public async Task<WeatherModel> GetWeatherForcastAsync(string url)
		{
			////http://api.openweathermap.org/data/2.5/forecast?zip=77007&APPID=c728d330cb85928cbea65ed959eb3fbf
		  //public const string OPEN_WEATHER_API_FORCAST_URL = "http://api.openweathermap.org/data/2.5/forecast?zip=";
			if (this.IsConnected())
			{
				try
				{
					
					var uri = new Uri(url);
					HttpClient client = new HttpClient();
					HttpResponseMessage response = await client.GetAsync(uri);
					String responseJson = await response.Content.ReadAsStringAsync();


					WeatherModel model = JsonConvert.DeserializeObject<WeatherModel>(responseJson);

					return model;
				}
				catch (Exception e)
				{
					//TODO add exception handling
					return null;
				}
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

