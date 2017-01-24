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
using Environment = Android.OS.Environment;
using File = Java.IO.File;

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
            Task startupWork = new Task(() =>
            {
                if (AndroidShared.CheckForInternetConnection()) {
                    if (!System.IO.File.Exists(_basePath + "/timestamps.json"))
                        FirstTimeSetup();
                    else
                        AndroidShared.Update();
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
            AndroidShared.Update();
        }

        private void CreateTimestap() {

            Dictionary<string, DateTime> timestaps = new Dictionary<string, DateTime> {
                {"Games", DateTime.MinValue},
                {"Products", DateTime.MinValue},
                {"Events", DateTime.MinValue},
                {"AboutUs", DateTime.MinValue}
            };

            System.IO.File.WriteAllText(_basePath + "/timestamps.json",JsonConvert.SerializeObject(timestaps));
        }

        public void SaveImage(string directory, string fileName, Bitmap bitmap) {
            string saveLocation = directory + fileName;
            using (FileStream os = new FileStream(saveLocation, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, os);
            }
        }

    }
}