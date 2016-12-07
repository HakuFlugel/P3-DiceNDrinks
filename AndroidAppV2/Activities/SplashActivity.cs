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
            SaveImagesToFolder(_basePath + "/images/");  
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
            //TODO: ino this is stupid, så i'm going to make a GetImages which gets them from resources so we don't need to do this ;) -Lighdek
        private void SaveImagesToFolder(string path) {
            SaveImage(path, "Top_Left_Games.png", OpenImage(Resource.Drawable.Top_Left_Games));
            SaveImage(path, "Top_Right_Menu.png", OpenImage(Resource.Drawable.Top_Right_Menu));
            SaveImage(path, "Bottom_Left_Reservation.png", OpenImage(Resource.Drawable.Bottom_Left_Reservation));
            SaveImage(path, "Bottom_Right_Events.png", OpenImage(Resource.Drawable.Bottom_Right_Events));
            SaveImage(path, "IconV3.png", OpenImage(Resource.Drawable.Iconv3));
        }

        private Bitmap OpenImage(int id)
        {
            return BitmapFactory.DecodeResource(Resources, id);
        }

        public void SaveImage(string directory, string fileName, Bitmap bitmap) {
            string saveLocation = directory + fileName;
            using (FileStream os = new FileStream(saveLocation, FileMode.CreateNew)) {
                bitmap.Compress(Bitmap.CompressFormat.Png, 50, os);
            }
        }

        private void Update() {
            string[] items = { "games", "products", "events" };
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
                    DownloadUpdate(items[i] + ".json");
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
        //TODO: Maybe async?
        private /*async*/ void DownloadUpdate(string location) { 
            string saveLocation = Path.Combine(Environment.ExternalStorageDirectory.Path, location);
            string item = "";
            MultipartFormDataContent content = new MultipartFormDataContent();
            //TODO: Make request for data
            System.IO.File.WriteAllText(saveLocation, item);

        }

        private DateTime[] AskServer() {
            DateTime[] newDateTimes = new DateTime[3];
            //TODO: ask server
            return newDateTimes;
        }
    }
}