using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using AvanadeStudioTV.Models;
using System.Xml.Serialization;
using System.IO;

namespace AvanadeStudioTV.Network
{
    public class FeedItemParser
    {
    

            public FeedItemParser()
            {
            }

        /*
         * public List<FeedItem> ParseFeed(string response)
             {
                 if (response == null)
                 {
                     return null;
                 }



                 XDocument doc = XDocument.Parse(response);
                 List<FeedItem> feeds = new List<FeedItem>();
                 foreach (var item in doc.Descendants("item"))
                 {
                     FeedItem feed = new FeedItem();
                     feed.title = item.Element("title").Value.ToString();
                     feed.link = item.Element("link").Value.ToString();
                     feed.description = item.Element("description").Value.ToString();
                     feed.pubdate = item.Element("pubDate").Value.ToString();
                     feed.guid = item.Element("guid").Value.ToString();
                     feeds.Add(feed);
                 }
                 return feeds;
             }

 */

        public List<Item> ParseFeed(string response)
        {
            if (response == null)
            {
                return null;
            }

            Rss FeedObject = new Rss();
            XmlSerializer serializer = new XmlSerializer(typeof(Rss));

            using (var reader = new StringReader(response))
            {
                FeedObject = (Rss)serializer.Deserialize(reader);
            }

            return FeedObject.Channel.Item;

        }

    }
}
