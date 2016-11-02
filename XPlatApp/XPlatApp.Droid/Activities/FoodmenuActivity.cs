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


namespace XPlatApp.Droid
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Menu")]
    public class FoodmenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.foodmenuLayout);
            // Create your application here

            ListView listView = (ListView)FindViewById(Resource.Id.listView1);

            //base adaptor should be food list
            //listView.Adapter = new ArrayAdapter<>();
            //TODO: ArrayAdapter to make listview

            FindViewById<Button>(Resource.Id.foodButton).Click += delegate
            {
                //todo: Fetch food list and set it as adapter
            };
            FindViewById<Button>(Resource.Id.drinkButton).Click += delegate
            {
                //todo: Fetch drink list and set it as adapter
            };
            FindViewById<Button>(Resource.Id.miscButton).Click += delegate
            {
                //todo: Fetch misc list and set it as adapter
            };
        }
    }
}