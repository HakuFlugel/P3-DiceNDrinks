using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Path = System.IO.Path;


namespace AndroidAppV2.Activities {
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Label = "Dice 'n Drinks",
         ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity {
        // ReSharper disable once InconsistentNaming
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        private readonly string _basePath = Environment.ExternalStorageDirectory.Path + "/DnD";

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState) {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override void OnResume() {
            base.OnResume();

            Task startupWork = new Task(() => {
                if (!System.IO.File.Exists(_basePath + "/timestamp.txt"))
                    FirstTimeSetup();
                    Update();
            });

            startupWork.ContinueWith(t => {
                Log.Debug(TAG, "Work is finished - start MainActivity");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

        private void FirstTimeSetup() {
            File folder = new File(_basePath + "/images");
            folder.Mkdirs();
            string timeStampPath = _basePath + "/timestamp.txt";
            File timestap = new File(timeStampPath);
            timestap.CreateNewFile();
            WriteDate(timeStampPath); 
        }

        private void WriteDate(string path)
        {
            using (
            StreamWriter sw =
            new StreamWriter(path)) {
                for (int i = 0; i < 3; i++) {
                    sw.WriteLine(DateTime.MinValue + "\r\n");
                }
            }
        }

        public void SaveImage(string directory, string fileName, Bitmap bitmap) {
            string saveLocation = directory + fileName;
            using (FileStream os = new FileStream(saveLocation, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, os);
            }
        }

        private void Update() {
            string[] items = { "Games", "Products", "Events" };
            DateTime[] loadedDateTimes = new DateTime[4];
            using (
                StreamReader sr =
                    new StreamReader(_basePath + "/timestamp.txt")
            ) {
                for (int i = 0; i < 3; i++) {
                    loadedDateTimes[i] = DateTime.Parse(sr.ReadLine());
                }
            }

            DateTime[] downloadedDateTimes = AskServer();

            for (int i = 0; i < 3; i++) {
                if (loadedDateTimes[i].Ticks < downloadedDateTimes[i].Ticks)
                    DownloadUpdate(items[i]);
            }
            SaveNewDate(downloadedDateTimes);
        }

        private void SaveNewDate(DateTime[] upDatedDateTime) {
            using (
                StreamWriter sw =
                new StreamWriter(_basePath + "/timestamp.txt")) {
                for (int i = 0; i < 3; i++) {
                    sw.WriteLine(upDatedDateTime[i]);
                }
            }
        }

        private void DownloadUpdate(string location) {
            AndroidShared ans = new AndroidShared(); 
            string saveLocation = Path.Combine(Environment.ExternalStorageDirectory.Path, $"{location}.json");
            string item;

            switch (location)
            {
                case "Products":
                    item = ans.DownloadProducts();
                    break;
                default:
                    item = ans.DownloadItem(location);
                    break;
            }

            System.IO.File.WriteAllText(saveLocation, item);

        }

        private DateTime[] AskServer() {
            DateTime[] newDateTimes = new DateTime[3];
            //TODO: ask server
            return newDateTimes;
        }
    }
}