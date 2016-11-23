using System;
using System.IO;
using Android.App;
using Android.Widget;
using Android.OS;

using AndroidAppV2.Activities;
using Newtonsoft.Json;
using Shared;

namespace AndroidAppV2
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Dice 'n Drinks", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private bool _doubleTapToExit = false;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            //DO NOT WRITE CODE ABOVE THIS LINE
            GC.Collect();
            
            FindViewById<ImageButton>(Resource.Id.gameButton).Click += delegate
            {
                StartActivity(typeof(GameActivity));
            };

            FindViewById<ImageButton>(Resource.Id.foodmenuButton).Click += delegate
            {
                StartActivity(typeof(ProductActivity));
            };

            FindViewById<ImageButton>(Resource.Id.reservationButton).Click += delegate
            {
                StartActivity(typeof(ReservationActivity));
            };

            FindViewById<ImageButton>(Resource.Id.eventButton).Click += delegate
            {
                StartActivity(typeof(EventActivity));
            };

            FindViewById<ImageButton>(Resource.Id.centerImageButton1).Click += delegate
            {
                StartActivity(typeof(ContactActivity));
            };
        }




        public override void OnBackPressed()
        {
            if (_doubleTapToExit)
                base.OnBackPressed();

            _doubleTapToExit = true;
            Toast.MakeText(this, "Please click BACK again to exit",ToastLength.Long).Show();

            /*AlertDialog.Builder exitApp = new AlertDialog.Builder(this);
            exitApp.SetMessage(Resource.String.exit);
            exitApp.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { base.OnBackPressed(); });
            exitApp.SetNegativeButton(Resource.String.no, (senderAlert, args) => { _doubleTapToExit = false; });
            Dialog exit = exitApp.Create();
            exit.Show();*/
        }
    }
}