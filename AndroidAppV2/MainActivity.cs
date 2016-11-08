using System;
using System.Reflection.Emit;
using Android.App;
using Android.Graphics.Pdf;
using Android.Widget;
using Android.OS;
using Android.Views;
using AndroidAppV2.Activities;
using Mono.Security.Interface;
using Xamarin.Forms;
using Shared;

namespace AndroidAppV2
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Dice 'n Drinks", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            FindViewById<ImageButton>(Resource.Id.gameButton).Click += delegate
            {
                StartActivity(typeof(GameActivity));
            };

            FindViewById<ImageButton>(Resource.Id.foodmenuButton).Click += delegate
            {
                StartActivity(typeof(FoodmenuActivity));
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
            AlertDialog.Builder exitApp = new AlertDialog.Builder(this);
            exitApp.SetMessage(Resource.String.exit);
            exitApp.SetPositiveButton(Resource.String.yes, (senderAlert, args) => { base.OnBackPressed(); });
            exitApp.SetNegativeButton(Resource.String.no, (senderAlert, args) => { /*Scratch Ass*/ });

            Dialog exit = exitApp.Create();

            exit.Show();

        }
    }
}

