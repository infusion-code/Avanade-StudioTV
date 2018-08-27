 
using System.Collections.Generic;
using System.Text;
using System;
using Realms;
using Xamarin.Forms;

namespace AvanadeStudioTV.Database
{
   public class RSSFeedData: RealmObject
	{
		public string ChannelName { get; set; }  //Title
		public string Desc { get; set; }
	
		public string url { get; set; }

		public string imageUrl { get; set; }

		public DateTimeOffset LastModified { get; set; }

		public bool isActiveFeed { get; set; }



	}

	//View Model Object (not presisted in Realm)
	public class RSSFeedViewData
	{
		public string ChannelName { get; set; }  //Title
		public string Desc { get; set; }

		public string url { get; set; }

		public string imageUrl { get; set; }

		public DateTimeOffset LastModified { get; set; }

		public bool isActiveFeed { get; set; }

		//UI Commands for Item:

		public Command<RSSFeedViewData> DeleteCommand { get; set; }

		public Command MakeActiveFeedCommand { get; set; }



	}
}
