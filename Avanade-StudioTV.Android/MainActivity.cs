using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Octane.Xamarin.Forms.VideoPlayer.Android;

namespace Avanade_StudioTV.Droid
{
    [Activity(Label = "Avanade_StudioTV", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            HtmlLabelRenderer.Initialize();
            global::Xamarin.Forms.Forms.Init(this, bundle);

            //init libraries
            Xamarin.Essentials.Platform.Init(this, bundle);

            //TODO Trial Mode only allows 15 seconds of playback see
            //https://github.com/adamfisher/Xamarin.Forms.VideoPlayer/blob/master/GettingStarted.md
            //FormsVideoPlayer.Init(); 

            LoadApplication(new App());
        }

        public override  void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


}

