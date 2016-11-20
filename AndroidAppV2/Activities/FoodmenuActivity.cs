using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;

using AndroidAppV2.ListAdapters;
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
            Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
            Spinner sectionSpinner = FindViewById<Spinner>(Resource.Id.sectionSpinner);
            ListView listView = FindViewById<ListView>(Resource.Id.list);
            List<Product> list = GenerateProductList();
            ProductAdapter adapter = new ProductAdapter(this, list);
            adapter.SetListType("Food"); // default view
            listView.Adapter = adapter;
            listView.ItemClick += OnListItemClick;

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
                adapter.SetList(adapter.GetSections()[0]);                  //Sets the list to correspond the chosen section.
            };
            sectionSpinner.ItemSelected += delegate
            {
                adapter.SetList((string)sectionSpinner.SelectedItem);
            };

        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;

            //todo: dialog fragment med yderligere info om et item
        }

        //functions from the adminpanel to generate a list of products
        private List<Product> GenerateProductList()
        {
            List<Product> productList = new List<Product>();
            PriceElement priceElementLittle = new PriceElement
            {
                name = "Little drink",
                price = 11
            };


            PriceElement priceElementBig = new PriceElement
            {
                name = "Big drink",
                price = 69
            };

            List<PriceElement> pricelist = new List<PriceElement> {priceElementLittle, priceElementBig};


            // food
            productList.Add(Prmake("gert", "ProductTest/_default.png", "Food", "Meat", pricelist));
            productList.Add(Prmake("shano", "ProductTest/_default.png", "Food", "Meat", pricelist));
            productList.Add(Prmake("Asger", "ProductTest/Red.png", "Food", "Meat", pricelist));
            productList.Add(Prmake("matias", "ProductTest/Red.png", "Food", "derp", pricelist));


            //drinks
            productList.Add(Prmake("1", "ProductTest/Red.png", "Drinks", "cola 1", pricelist));
            productList.Add(Prmake("2", "ProductTest/Red.png", "Drinks", "coffe 2", pricelist));
            productList.Add(Prmake("3", "ProductTest/Red.png", "Drinks", "stuff", pricelist));
            productList.Add(Prmake("4", "ProductTest/Red.png", "Drinks", "other", pricelist));


            // misc.
            productList.Add(Prmake("5", "ProductTest/Red.png", "Misc", "1", pricelist));
            productList.Add(Prmake("6", "ProductTest/Red.png", "Misc", "2", pricelist));
            productList.Add(Prmake("7", "ProductTest/Red.png", "Misc", "3", pricelist));
            productList.Add(Prmake("8", "ProductTest/Red.png", "Misc", "4", pricelist));

            return productList;
        }

        private static Product Prmake(string name, string image, string cat, string section, List<PriceElement> pl)
        {
            Product pr = new Product
            {
                name = name,
                image = image,
                category = cat,
                section = section,
                PriceElements = pl
            };

            return pr;
        }
    }
}