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
		public static FeedManager DataManager { get; set; }
		public App()
        {
            InitializeComponent();

			InitalizeData();

		    

			if ((bool)DataManager.IsFullScreenView)
			{

				MainPage = new    FullScreenVideoPage(); //new SettingsPage(null); // FullScreenListPage(); //new FullScreenVideoPage();

			}

			else
			{
				MainPage = new MasterPage();
			}
        }

		//using Realm to persist data - initalize if no feeds created by user first:
		private void InitalizeData()
		{
        	DataManager = FeedManager.Instance;

		}
 

		protected override void OnStart()
        {
			 DataManager.GetDataFromNetwork();
			 DataManager.TitleAnimationState = TitleAnimationStatus.NotPlaying;
        }

        protected override void OnSleep()
        {
          
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
