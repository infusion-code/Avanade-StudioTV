 
using System.Collections.Generic;
using System.Text;
using System;
using Realms;

namespace AvanadeStudioTV.Database
{
    class RSSFeedData: RealmObject
	{
		public string ChannelName { get; set; }
		public string Desc { get; set; }
	
		public string url { get; set; }

		public DateTimeOffset LastModified { get; set; }

		 
 
	}
}
