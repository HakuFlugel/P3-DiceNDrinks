using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Android;

namespace AndroidApp.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Reservation")]
    public class ReservationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.ReservationLayout);
            // Create your application here
            DatePicker calender = FindViewById<DatePicker>(Resource.Id.datePicker1);
            TimePicker clock = FindViewById<TimePicker>(Resource.Id.timePicker1);
            Button button = FindViewById<Button>(Resource.Id.button1);
            //calender.MinDateTime = System.DateTime.Today;
            button.Click += delegate
            {
                button.Text =
                    $"{calender.DayOfMonth}." +
                    $"{calender.Month}." +
                    $"{calender.Year}" +
                    $" {clock.Hour}:" +
                    $"{clock.Minute}";
            };
        }
    }
}