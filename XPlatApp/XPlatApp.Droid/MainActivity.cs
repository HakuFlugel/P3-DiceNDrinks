using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Android;
using Shared;

namespace XPlatApp.Droid
{
	[Activity (Label = "XPlatApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Forms.Init (this, bundle);
			LoadApplication (new App());
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

