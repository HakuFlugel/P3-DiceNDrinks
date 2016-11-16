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
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games")]
    public class GameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.GameLayout);
            // Create your application here
            ListView listView = (ListView)FindViewById(Resource.Id.gameListView);

            //base adaptor should be random? list
            //listView.Adapter = new ArrayAdapter<>();
            //TODO: Her skal listen med menugenstande linkes til en ArrayAdapter så de kan vises i appen
            //TODO: derudover skal der også laves funktionalitet til at sortere listen...
            //https://developer.xamarin.com/recipes/android/data/adapters/use_an_arrayadapter/
            //https://developer.xamarin.com/api/type/Xamarin.Forms.ListView/

            FindViewById<Button>(Resource.Id.playerButton).Click += delegate
            {
                //todo: Fetch food list and set it as adapter
            };
            FindViewById<Button>(Resource.Id.difficultyButton).Click += delegate
            {
                //todo: Fetch drink list and set it as adapter
            };
            FindViewById<Button>(Resource.Id.gametimeButton).Click += delegate
            {
                //todo: Fetch misc list and set it as adapter
            };

        }
    }
}