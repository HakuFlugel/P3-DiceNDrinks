//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Android.App;
//using Android.Views;
//using Android.Widget;

//using Shared;

//namespace AndroidAppV2.ListAdapters
//{
//    internal class ProductAdapter : BaseAdapter<Product>
//    {
//        private bool _default = true;
//        private List<Product> _items;
//        private List<Product> _categoryItems;
//        private readonly List<Product> _baseItems;
//        private readonly Activity _context;

//        public ProductAdapter(Activity context, List<Product> items)
//        {
//            _context = context;
//            _baseItems = _items = items;
//            Sort();
//        }

//        public override long GetItemId(int position)
//        {
//            return position;
//        }

//        public override View GetView(int position, View convertView, ViewGroup parent)
//        {
//            Product item = _items[position];
//            AndroidShared an = new AndroidShared();
//            //sets the view as convertView unless convertView is null
//            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.productListItem, null);
//            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
//            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"From {item.PriceElements[0].price} kr.";
//            int[] sizes = {75, 75};
//            an.GetImagesFromAssets(_context, $"ProductPics/{item.image}.png",view, Resource.Id.Image,sizes);
//            return view;
//        }

//        public override int Count => _items.Count;

//        public override Product this[int position] => _items[position];

//        public void SetListType(string type)
//        {
//            if (type != "Alt")
//            {
//                _categoryItems = _baseItems.Where(prd => prd.category == type).ToList();
//                _default = false;
//            }
//            else
//            {
//                _categoryItems = _baseItems;
//                _default = true;
//            }
//        }

//        public List<string> GetSections()
//        {
//            List<string> sections = new List<string>();

//            if (_default)
//            {
//                sections.Add("Alt");
//                return sections;
//            }

//            foreach (var item in _categoryItems)
//            {
//                if (!sections.Any(x => x.Contains(item.section)))
//                sections.Add(item.section);
//            }


//            if (sections.Count != 0)
//                return sections;

//            sections.Add("Ingenting");
//                return sections;
//        }

//        public void SetList(string section)
//        {
//            if (!_default)
//                _items = _baseItems.Where(prd => prd.section == section).ToList();
//            else
//                _items = _baseItems;
//            NotifyDataSetChanged();
//            GC.Collect();
//        }

//        private void Sort()
//        {
//            _baseItems.Sort((a,b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
//        }

//        public Product GetProductByPosition(int position)
//        {
//            return _items[position];
//        }

//    }
//}


/////// ProductActivity stuff
/*
expListView.ItemClick += (s, e) => {
    Product theProduct = adapter.GetProductByPosition(e.Position);

    var dialog = new ProductDialogFragment();
    dialog.PassDataToFrag(theProduct, this);
    dialog.Show(FragmentManager, "Produkt Dialog");
};

var categorySpinnerAdapter = ArrayAdapter.CreateFromResource(
    this, Resource.Array.categoryspinner, Android.Resource.Layout.SimpleSpinnerItem);

ArrayAdapter sectionSpinnerAdapter = new ArrayAdapter<string>(this,Android.Resource.Layout.SimpleSpinnerItem, 
adapter.GetSections());


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
*/