using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using AndroidAppV2.Activities;

namespace AndroidAppV2 {
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Dice 'n Drinks", Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        private bool _doubleTapToExit;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            SetImages();
            GC.Collect();

            FindViewById<ImageButton>(Resource.Id.gameButton).Click += delegate {
                Log.WriteLine(LogPriority.Info, typeof(MainActivity).Name, $"Opening {typeof(GameActivity).Name}");
                StartActivity(typeof(GameActivity));
            };

            FindViewById<ImageButton>(Resource.Id.foodmenuButton).Click += delegate {
                Log.WriteLine(LogPriority.Info, typeof(MainActivity).Name, $"Opening {typeof(ProductActivity).Name}");
                StartActivity(typeof(ProductActivity));
            };

            FindViewById<ImageButton>(Resource.Id.reservationButton).Click += delegate {
                Log.WriteLine(LogPriority.Info, typeof(MainActivity).Name, $"Opening {typeof(ReservationActivity).Name}");
                StartActivity(typeof(ReservationActivity));
            };

            FindViewById<ImageButton>(Resource.Id.eventButton).Click += delegate {
                Log.WriteLine(LogPriority.Info, typeof(MainActivity).Name, $"Opening {typeof(EventActivity).Name}");
                StartActivity(typeof(EventActivity));
            };

            FindViewById<ImageButton>(Resource.Id.centerImageButton1).Click += delegate {
                Log.WriteLine(LogPriority.Info, typeof(MainActivity).Name, $"Opening {typeof(ContactActivity).Name}");
                StartActivity(typeof(ContactActivity));
            };
        }
        
        private void SetImages() {
            AndroidShared an = new AndroidShared();
            DisplayMetrics metrics = Resources.DisplayMetrics;
            int widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            int heightInDp = ConvertPixelsToDp(metrics.HeightPixels);
            int[] sizes = { widthInDp / 2, heightInDp / 2 };
            
            an.GetImages(this, "Top_Left_Games.png", FindViewById<ImageButton>(Resource.Id.gameButton), Resource.Id.gameButton, sizes);
            an.GetImages(this, "Top_Right_Menu.png", FindViewById<ImageButton>(Resource.Id.foodmenuButton), Resource.Id.foodmenuButton, sizes);
            an.GetImages(this, "Bottom_Left_Reservation.png", FindViewById<ImageButton>(Resource.Id.reservationButton), Resource.Id.reservationButton, sizes);
            an.GetImages(this, "Bottom_Right_Events.png", FindViewById<ImageButton>(Resource.Id.eventButton), Resource.Id.eventButton, sizes);
            an.GetImages(this, "Iconv3.png", FindViewById<ImageButton>(Resource.Id.centerImageButton1), Resource.Id.centerImageButton1, new[] { widthInDp / 2, widthInDp / 2 });
        }

        public override void OnRestoreInstanceState(Bundle savedInstanceState, PersistableBundle persistentState) {
            _doubleTapToExit = false;
            base.OnRestoreInstanceState(savedInstanceState, persistentState);
        }

        private int ConvertPixelsToDp(float pixelValue) {
            int dp = (int)(pixelValue / Resources.DisplayMetrics.Density);
            return dp;
        }

        public override void OnBackPressed() {
            if (_doubleTapToExit)
                base.OnBackPressed();

            _doubleTapToExit = true;
            Toast.MakeText(this, Resource.String.exit, ToastLength.Long).Show();
        }

        protected override void OnResume()
        {
            base.OnResume();
            AndroidShared.Update();
        }
    }
}