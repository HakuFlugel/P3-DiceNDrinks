using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Newtonsoft.Json;
using Shared;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
using Path = System.IO.Path;

namespace AndroidAppV2.Activities {
    [Activity(Theme = "@style/Theme.NoTitle", MainLauncher = true, NoHistory = true, Label = "Dice 'n Drinks",
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
            SetContentView(Resource.Layout.Splash);
            Task startupWork = new Task(() => {
                if (!System.IO.File.Exists(_basePath))
                    FirstTimeSetup();
                if (AndroidShared.CheckForInternetConnection()) {
                    Update();
                }                
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
            CreateTimestap();
        }

        private void CreateTimestap() {

            Dictionary<string, DateTime> timestaps = new Dictionary<string, DateTime> {
                {"Games", DateTime.MinValue},
                {"Products", DateTime.MinValue},
                {"Events", DateTime.MinValue}
            };

            System.IO.File.WriteAllText(_basePath + "/timestamps.json",JsonConvert.SerializeObject(timestaps));
        }

        public void SaveImage(string directory, string fileName, Bitmap bitmap) {
            string saveLocation = directory + fileName;
            using (FileStream os = new FileStream(saveLocation, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, os);
            }
        }

        private void Update() {
            string[] items = { "Events", "Games", "Products", "AboutUs"};

            Dictionary<string, DateTime> newTimes = DownloadTimestap();
            Dictionary<string, DateTime> oldTimes = LocalTimestap();

            foreach (string item in items) {
                if (newTimes[item].Ticks > oldTimes[item].Ticks) {
                    DownloadUpdate(item);
                }
            }
            UpdateTimestap(newTimes);
        }

        private Dictionary<string, DateTime> DownloadTimestap() {
            return JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(AndroidShared.DownloadItem("timestamps"));
        }

        private Dictionary<string, DateTime> LocalTimestap()
        {
            string file = System.IO.File.ReadAllText(_basePath + "/timestamps.json");
            
            return JsonConvert.DeserializeObject<Dictionary<string,DateTime>>(file);
        }

        private void UpdateTimestap(Dictionary<string, DateTime> newTimes) {
            System.IO.File.WriteAllText(_basePath + "/timestamps.json", JsonConvert.SerializeObject(newTimes));
        }

        private void DownloadUpdate(string location) {
            string saveLocation = Path.Combine(Environment.ExternalStorageDirectory.Path, $"{_basePath}/{location}.json");
            string item;

            switch (location) {
                case "Products":
                    item = AndroidShared.DownloadProducts();
                    System.IO.File.WriteAllText(saveLocation, item);
                    DownloadProductImages(item);
                    break;
                case "Games":
                    item = AndroidShared.DownloadItem(location);
                    System.IO.File.WriteAllText(saveLocation, item);
                    DownloadGameImages(item);
                    break;
                case "Events":
                    item = AndroidShared.DownloadItem(location);
                    System.IO.File.WriteAllText(saveLocation, item);
                    break;
                case "AboutUs":
                    item = AndroidShared.DownloadItem(location);
                    System.IO.File.WriteAllText(saveLocation, item);
                    break;
            }

        }

        private static void DownloadGameImages(string jsonlist) {
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(jsonlist);

            foreach (Game item in list) {
                AndroidShared.ImageDownloader(item.imageName, "games");
            }
        }

        private static void DownloadProductImages(string jsonlist) {
            List<Product> list = JsonConvert.DeserializeObject<List<Product>>(jsonlist);

            foreach (Product item in list) {
                AndroidShared.ImageDownloader(item.image, "products");
            }
        }
    }
}