using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Environment = Android.OS.Environment;
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

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                if (!System.IO.File.Exists(Path.Combine(Environment.ExternalStorageDirectory.Path, "DnD")))
                    FirstTimeSetup();
                else
                {
                    Update();
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
            File timestap = new File(Environment.ExternalStorageDirectory.Path + "/DnD/timestamp.txt");
            timestap.CreateNewFile();
            using (
                StreamWriter sw =
                    new StreamWriter(Path.Combine(Environment.ExternalStorageDirectory.Path, "/DnD/timestamp.txt")))
            {
                for (int i = 0; i < 3; i++)
                {
                    sw.WriteLine(DateTime.Today.ToLongDateString());
                }
            }

        }

        private void Update()
        {
            string[] items = {"games","products","events"};
            DateTime[] loadedDateTimes = new DateTime[4];
            using (
                StreamReader sr =
                    new StreamReader(Path.Combine(Environment.ExternalStorageDirectory.Path, "/DnD/timestamp.txt"))
            )
            {
                for (int i = 0; i < 3; i++)
                {
                    loadedDateTimes[i] = DateTime.Parse(sr.ReadLine());
                }
            }


            DateTime[] downloadedDateTimes = AskServer();


            for (int i = 0; i < 3; i++)
            {
                if (loadedDateTimes[i].Ticks < downloadedDateTimes[i].Ticks)
                    DownloadUpdate(items[i] + ".json");
            }
            SaveNewDate(downloadedDateTimes);

        }

        private void SaveNewDate(DateTime[] upDatedDateTime)
        {
            using (
                StreamWriter sw =
                new StreamWriter(Path.Combine(Environment.ExternalStorageDirectory.Path, "/DnD/timestamp.txt")))
            {
                for (int i = 0; i < 3; i++)
                {
                    sw.WriteLine(upDatedDateTime[i].ToLongDateString());
                }

            }
        }

        private /*async*/ void DownloadUpdate(string location) //TODO: Maybe async?
        {
            string saveLocation = Path.Combine(Environment.ExternalStorageDirectory.Path, location);
            string item = "";
            MultipartFormDataContent content = new MultipartFormDataContent();
            //TODO: Make request for data
            System.IO.File.WriteAllText(saveLocation, item);

        }

        private DateTime[] AskServer()
        {
            DateTime[] newDateTimes = new DateTime[3];
            //TODO: ask server
            return newDateTimes;
        }

    }
}