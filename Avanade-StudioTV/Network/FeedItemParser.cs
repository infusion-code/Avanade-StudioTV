using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using AvanadeStudioTV.Models;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

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

           // FeedObject = ScrubObject(FeedObject);
            return FeedObject.Channel.Item;

        }

        private Rss ScrubObject(Rss feedObject)
        {
            Rss FeedObject = new Rss();

            FeedObject.Channel = new Channel();
            FeedObject.Channel.Item = new List<Item>();

            foreach (var item in feedObject.Channel.Item)
            {
                var nItem = new Item();
                nItem = item;
                nItem.Thumbnail = new List<Thumbnail>();
                nItem.Thumbnail.Add(item.Thumbnail.Find( t=>t.Width == "220"));
                FeedObject.Channel.Item.Add(nItem);
            }

            return FeedObject;
        }
    }
}
