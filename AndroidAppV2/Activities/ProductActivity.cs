using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Label = "Menu", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : Activity
    {

        List<Product> list;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            
            
            SetContentView(Resource.Layout.productLayout);

            // Create your application here
            Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
            ExpandableListView expListView = FindViewById<ExpandableListView>(Resource.Id.list);

            var categorySpinnerAdapter = ArrayAdapter.CreateFromResource(
                            this, Resource.Array.categoryspinner, Android.Resource.Layout.SimpleSpinnerItem);

            categorySpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categorySpinner.Adapter = categorySpinnerAdapter;

            //List<Product> list = GenerateProductList();
            list = GetProducts();

            expListView.SetAdapter(new ExpandableDataAdapter(this, list, GetGroups(list, (string)categorySpinner.SelectedItem)));

            categorySpinner.ItemSelected += delegate {
                //adapter.SetListType((string)categorySpinner.SelectedItem);  //Sets the category of the list to the chosen item
                expListView.SetAdapter(new ExpandableDataAdapter(this, list, GetGroups(list, (string)categorySpinner.SelectedItem)));
            };
        }

        private List<Product> GetProducts()
        {
            List<Product> list;
            AndroidShared.LoadData(this,"products.json", out list);

            return list;
        }
        
        private List<string> GetGroups(List<Product> productList, string category) {
            List<string> list;
            List<Product> tempList = productList.FindAll(o => o.category == category);
            list = tempList.Select(o => o.section).Distinct().ToList();

            return list;
        } 
    }
}