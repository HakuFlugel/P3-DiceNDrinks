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

namespace AndroidApp.Activities
{
    [Activity(Label = "Menu")]
    public class FoodmenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.foodmenuLayout);
            // Create your application here
        }
    }
}