using System;
using AvanadeStudioTV.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Realms;
using AvanadeStudioTV.Database;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Avanade_StudioTV
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			InitalizeData();

            MainPage = new MasterPage();
        }

		//using Realm to persist data - initalize if no feeds created by user first:
		private void InitalizeData()
		{
			var realm = Realm.GetInstance();
			var RssFeeds = realm.All<RSSFeedData>();

			if (RssFeeds.AsRealmCollection<RSSFeedData>().Count <1)
			{
				var DefaultFeed = new RSSFeedData()
				{
					ChannelName = "Xamarin Show",
					Desc = "Xamarin TV RSS Feed",
					url = "https://s.ch9.ms/Shows/XamarinShow/feed/mp4",
					LastModified = new DateTimeOffset(DateTime.Now)

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

		protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
