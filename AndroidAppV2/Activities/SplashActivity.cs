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
        }

        public void SaveImage(string directory, string fileName, Bitmap bitmap) {
            string saveLocation = directory + fileName;
            using (FileStream os = new FileStream(saveLocation, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, os);
            }
        }

        private void Update() {
            string[] items = { "Games", "Products", "Events" };

            for (int i = 0; i < 3; i++) {
                //TODO: enable when we can get timestamp for updates
                    DownloadUpdate(items[i]);
            }
        }

        private void DownloadUpdate(string location) {
            AndroidShared ans = new AndroidShared(); 
            string saveLocation = Path.Combine(Environment.ExternalStorageDirectory.Path, $"{_basePath}/{location}.json");
            string item;

            switch (location)
            {
                case "Products":
                    item = ans.DownloadProducts();
                    DownloadProductImages(item);
                    break;
                case "Games":
                    item = ans.DownloadItem(location);
                    DownloadGameImages(item);
                    break;
                case "Events":
                    item = ans.DownloadItem(location);
                    break;
                default:
                    item = "";
                    break;
            }
            System.IO.File.WriteAllText(saveLocation, item);
        }

        private static void DownloadGameImages(string jsonlist)
        {
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(jsonlist);

            foreach (Game item in list)
            {
                new AndroidShared.ImageDownloader(item.imageName, "games");
            }
        }

        private static void DownloadProductImages(string jsonlist) {
            List<Product> list = JsonConvert.DeserializeObject<List<Product>>(jsonlist);

            foreach (Product item in list) {
                new AndroidShared.ImageDownloader(item.image, "products");
            }
        }
    }
}