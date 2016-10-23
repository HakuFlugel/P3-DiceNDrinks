using System.Reflection.Emit;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using AndroidApp.Activities;
using Shared;

namespace AndroidApp
{
    [Activity(Label = "Dice 'n Drinks", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
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
    }
}