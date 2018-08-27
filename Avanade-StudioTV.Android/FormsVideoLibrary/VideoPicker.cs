using System;
using System.Threading.Tasks;
using Android.Content;
using Xamarin.Forms;
using Avanade_StudioTV.Droid;
 
// Need application's MainActivity
 


/*=================
 * 
 * 
 * 
 *  Aug 17 2018:  NOT USED in iOS  or ANDROID - Currently we are using a Web View to load/play video on iOS and Android 
 *   Reason: webview works well to load a skinnable video player on mobile platforms EXCEPT UWP - on UWP needed to use the native Media Element 
 * 
 * 
 * ===================
 */

[assembly: Dependency(typeof(FormsVideoLibrary.Droid.VideoPicker))]

namespace FormsVideoLibrary.Droid
{
    public class VideoPicker : IVideoPicker
    {
        public Task<string> GetVideoFileAsync()
        {
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("video/*");
            intent.SetAction(Intent.ActionGetContent);

            // Get the MainActivity instance
            MainActivity activity = MainActivity.Current;

            // Start the picture-picker activity (resumes in MainActivity.cs)
            activity.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Video"),
                MainActivity.PickImageId);

            // Save the TaskCompletionSource object as a MainActivity property
            activity.PickImageTaskCompletionSource = new TaskCompletionSource<string>();

            // Return Task object
            return activity.PickImageTaskCompletionSource.Task;
        }
    }
}