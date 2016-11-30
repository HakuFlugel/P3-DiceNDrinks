
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using Android.Support.V7.App;
using Android.Util;
using File = Java.IO.File;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Label = "Dice 'n Drinks",
         ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        // ReSharper disable once InconsistentNaming
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override async void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                if (!System.IO.File.Exists(Path.Combine(Environment.ExternalStorageDirectory.Path, "DnD")))
                    FirstTimeSetup();
                else if (CheckForUpdate())
                {
                    DownloadUpdate();
                }

            });

            startupWork.ContinueWith(t =>
            {
                Log.Debug(TAG, "Work is finished - start MainActivity");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

        private static void FirstTimeSetup()
        {
            File folder = new File(Environment.ExternalStorageDirectory.Path + "/DnD/images");
            folder.Mkdirs();
        }

        private bool CheckForUpdate()
        {
            bool update = false;
            //TODO: ask server for update
            return update;
        }

        private void DownloadUpdate()
        {
            //TODO: Download games/menu here

        }

    }
}