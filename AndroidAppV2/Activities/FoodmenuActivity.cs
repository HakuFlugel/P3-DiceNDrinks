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

            ListView listView = (ListView)FindViewById(Resource.Id.listView1);

            //base adaptor should be food list
            //listView.Adapter = new ArrayAdapter<>();

            //TODO: Her skal listen med menugenstande linkes til en ArrayAdapter s� de kan vises i appen
            //https://developer.xamarin.com/recipes/android/data/adapters/use_an_arrayadapter/
            //https://developer.xamarin.com/api/type/Xamarin.Forms.ListView/

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