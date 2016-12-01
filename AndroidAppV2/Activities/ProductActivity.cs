using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;
using System;

namespace AndroidAppV2.Activities
{
    [Activity(Label = "Menu", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : Activity
    {
        private List<Product> _list;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.productLayout);

            // Create your application here
            _list = GetProducts();

            Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
            ExpandableListView expListView = FindViewById<ExpandableListView>(Resource.Id.list);

            //var categorySpinnerAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.categoryspinner, Android.Resource.Layout.SimpleSpinnerItem);

            ArrayAdapter categorySpinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem,GetCategories(_list));

            categorySpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categorySpinner.Adapter = categorySpinnerAdapter;


            //List<Product> list = GenerateProductList();
            

            ExpandableDataAdapter adapter = new ExpandableDataAdapter(this, _list, GetGroups(_list, (string)categorySpinner.SelectedItem));

            expListView.SetAdapter(adapter);
            for (int i = 0; i < GetGroups(_list, (string)categorySpinner.SelectedItem).Count; i++) {
                expListView.ExpandGroup(i);
            }
            categorySpinner.ItemSelected += delegate {
                //adapter.SetListType((string)categorySpinner.SelectedItem);  //Sets the category of the list to the chosen item
                expListView.SetAdapter(adapter = new ExpandableDataAdapter(this, _list, GetGroups(_list, (string)categorySpinner.SelectedItem)));
                for (int i = 0; i < GetGroups(_list, (string)categorySpinner.SelectedItem).Count; i++) {
                    expListView.ExpandGroup(i);
                }
                GC.Collect();
            };

            expListView.ChildClick += (s, e) => {
                Product theProduct = adapter.GetTheProduct(e.GroupPosition, e.ChildPosition);

                var dialog = new ProductDialogFragment();
                dialog.PassDataToFrag(theProduct, this);
                dialog.Show(FragmentManager, "Produkt Dialog");
                GC.Collect();
            };

        }

        private List<Product> GetProducts()
        {
            List<Product> list;
            AndroidShared.LoadData(this,"products.json", out list);

            return list;
        }
        
        private List<string> GetGroups(List<Product> productList, string category) {
            List<Product> tempList = productList.FindAll(o => o.category == category);
            List<string> list = tempList.Select(o => o.section).Distinct().ToList();

            return list;
        }
        private List<string> GetCategories(List<Product> productlist) {
            List<string> list = productlist.Select(o => o.category).Distinct().ToList();

            return list;
        } 
    }
}