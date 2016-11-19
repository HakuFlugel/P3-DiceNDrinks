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
using AndroidAppV2.LabeledSections;
using Product = AndroidAppV2.LabeledSections.Product;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Menu")]
    public class FoodmenuActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.foodmenuLayout);

            // Create your application here
            var data = new ListItemCollection<Product>() {
                new Product("derp","faggot","faggotass")
            };

            var sortedContacts = data.GetSortedData();
            var adapter = CreateAdapter(sortedContacts);
            ListAdapter = adapter;

            /*
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
            };*/
        }
        SeparatedListAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects)
    where T : IHasLabel, IComparable<T>
        {
            var adapter = new SeparatedListAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var label = e.Key;
                var section = e.Value;
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.ListItem, section));
            }
            return adapter;
        }


    }
}