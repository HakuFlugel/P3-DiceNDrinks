using System.Collections.Generic;
using System.Linq;
using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;


namespace AndroidAppV2.Activities {
    [Activity(Label = "Menu", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductActivity : Activity {
        private List<Product> _list;


        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.productLayout);

            Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinner);
            ExpandableListView expListView = FindViewById<ExpandableListView>(Resource.Id.list);

            ArrayAdapter categorySpinnerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, GetCategories(_list));

            categorySpinnerAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categorySpinner.Adapter = categorySpinnerAdapter;

            ExpandableDataAdapter adapter = new ExpandableDataAdapter(this, _list, GetGroups(_list, (string)categorySpinner.SelectedItem));

            expListView.SetAdapter(adapter);
            for (int i = 0; i < GetGroups(_list, (string)categorySpinner.SelectedItem).Count; i++) {
                expListView.ExpandGroup(i);
            }
            categorySpinner.ItemSelected += delegate {
                expListView.SetAdapter(adapter = new ExpandableDataAdapter(this, _list, GetGroups(_list, (string)categorySpinner.SelectedItem)));
                for (int i = 0; i < GetGroups(_list, (string)categorySpinner.SelectedItem).Count; i++) {
                    expListView.ExpandGroup(i);
                }
                GC.Collect();
            };

            expListView.ChildClick += (s, e) => {
                Product theProduct = adapter.GetTheProduct(e.GroupPosition, e.ChildPosition);

                ProductDialogFragment dialog = new ProductDialogFragment();
                dialog.PassDataToFrag(theProduct, this);
                dialog.Show(FragmentManager, "Product Dialog");
                GC.Collect();
            };

        }

        private List<Product> GetProducts() {
            List<Product> list;
            AndroidShared.LoadData(this, "products.json", out list);

            return list;
        }

        private static List<string> GetGroups(List<Product> productList, string category) {
            List<Product> tempList = productList.FindAll(o => o.category == category);
            return tempList.Select(o => o.section).Distinct().ToList();
        }
        private static List<string> GetCategories(List<Product> productlist) {
            return productlist.Select(o => o.category).Distinct().ToList();
        }
    }
}