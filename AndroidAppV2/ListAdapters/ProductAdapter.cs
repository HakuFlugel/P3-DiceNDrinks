using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using AndroidAppV2.Activities;

using Shared;

namespace AndroidAppV2.ListAdapters
{
    class ProductAdapter : BaseAdapter<Product>
    {
        private List<Product> _items;
        private readonly List<Product> _baseItems;
        private readonly Activity _context;
        private FoodmenuActivity _foodmenuActivity;

        public ProductAdapter(Activity context, List<Product> items)
        {
            _context = context;
            _baseItems = _items = items;
            Sort();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Product item = _items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.PriceElements[0].name}: {item.PriceElements[0].price}";
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageDrawable(AdapterShared.DLImage(_context,item.image));
            return view;
        }

        public override int Count => _items.Count;

        public override Product this[int position] => _items[position];

        public void SetListType(string type)
        {
            _items = _baseItems.Where(prd => prd.category == type).ToList();
        }

        public List<string> GetSections()
        {
            List<string> sections = new List<string>();

            foreach (var item in _items)
            {
                if (!sections.Any(x => x.Contains(item.section)))
                sections.Add(item.section);
            }
            return sections;
        }
        
        public void SetList(string section)
        {
            _items = _baseItems.Where(prd => prd.section == section).ToList();
            NotifyDataSetChanged();
        }

        private void Sort()
        {
            _baseItems.Sort((a,b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        }
    }
}