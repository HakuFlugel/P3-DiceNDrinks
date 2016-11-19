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
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Menu")]
    public class FoodmenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.foodmenuLayout);
            
            // Create your application here

            
            FindViewById<Button>(Resource.Id.foodButton).Click += delegate
            {

                //todo: update list
            };
            FindViewById<Button>(Resource.Id.drinkButton).Click += delegate
            {

                //todo: update list
            };
            FindViewById<Button>(Resource.Id.miscButton).Click += delegate
            {

                //todo: update list
            };
        }


    }
}