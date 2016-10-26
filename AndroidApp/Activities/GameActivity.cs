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
using Java.Util;

namespace AndroidApp.Activities
{
    [Activity(Label = "Games")]
    public class GameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.GameLayout);
            // Create your application here
            FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                FindViewById<Button>(Resource.Id.button1).Text = Locale.Default.DisplayLanguage;
            };

        }
    }
}