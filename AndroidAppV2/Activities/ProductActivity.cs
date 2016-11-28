using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Menu", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            
            
            SetContentView(Resource.Layout.productLayout);

            // Create your application here
            Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
            Spinner sectionSpinner = FindViewById<Spinner>(Resource.Id.sectionSpinner);
            ListView listView = FindViewById<ListView>(Resource.Id.list);
            //List<Product> list = GenerateProductList();
            List<Product> list = GetProducts();
            ProductAdapter adapter = new ProductAdapter(this, list);
            adapter.SetListType("Alt"); // default view
            listView.Adapter = adapter;

            listView.ItemClick += (s, e) => {
                Product theProduct = adapter.GetProductByPosition(e.Position);

                var dialog = new ProductDialogFragment();
                dialog.PassDataToFrag(theProduct, this);
                dialog.Show(FragmentManager, "Produkt Dialog");
            };

            var categorySpinnerAdapter = ArrayAdapter.CreateFromResource(
                this, Resource.Array.categoryspinner, Android.Resource.Layout.SimpleSpinnerItem);

            ArrayAdapter sectionSpinnerAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleSpinnerItem, adapter.GetSections());


            categorySpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categorySpinner.Adapter = categorySpinnerAdapter;
            sectionSpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            sectionSpinner.Adapter = sectionSpinnerAdapter;

            categorySpinner.ItemSelected += delegate
            {
                adapter.SetListType((string)categorySpinner.SelectedItem);  //Sets the category of the list to the chosen item
                sectionSpinnerAdapter.Clear();                              //Removes all current items from the spinner list
                sectionSpinnerAdapter.AddAll(adapter.GetSections());        //Adds all item associated with the chosen category
                sectionSpinner.SetSelection(0);                             //Selects the topmost item (because this isn't normal behavior)
                if (adapter.GetSections().Count != 0)
                adapter.SetList(adapter.GetSections()[0]);                  //Sets the list to correspond the chosen section.

            };
            sectionSpinner.ItemSelected += delegate
            {
                adapter.SetList((string)sectionSpinner.SelectedItem);
            };

        }

        private List<Product> GetProducts()
        {
            List<Product> list;
            AndroidShared.LoadData(this,"products.json", out list);

            return list;
        } 
    }
}