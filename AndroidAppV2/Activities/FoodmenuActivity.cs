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
            List<Product> list = getProducts("food");
            ItemAdapter itemAdapter = new ItemAdapter(this, list);
            ListView listView = (ListView)FindViewById(Resource.Id.listView1);
            listView.Adapter = itemAdapter;

            listView.ItemClick += OnListItemClick;
            
            //TODO: Her skal listen med menugenstande linkes til en ArrayAdapter så de kan vises i appen
            //TODO: Dette er igangsat i funktionen ItemAdapter
            //https://developer.xamarin.com/recipes/android/data/adapters/use_an_arrayadapter/
            //https://developer.xamarin.com/api/type/Xamarin.Forms.ListView/

            FindViewById<Button>(Resource.Id.foodButton).Click += delegate
            {
                list = getProducts("food");
                //todo: update list
            };
            FindViewById<Button>(Resource.Id.drinkButton).Click += delegate
            {
                list = getProducts("drink");
                //todo: update list
            };
            FindViewById<Button>(Resource.Id.miscButton).Click += delegate
            {
                list = getProducts("misc");
                //todo: update list
            };
        }

        List<Product> getProducts(string section)
        {
            List<Product> list = new List<Product>();

            //todo: get the products here

            return list;
        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            //todo: dialog fragment med yderligere info om et item
        }
    }
}