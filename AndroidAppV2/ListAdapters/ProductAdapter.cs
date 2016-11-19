using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidAppV2.Activities;
using Java.IO;
using Shared;

namespace AndroidAppV2.ListAdapters
{
    class ProductAdapter : BaseAdapter<Product>
    {
        List<Product> items;
        private List<Product> baseItems;
        Activity context;
        private FoodmenuActivity foodmenuActivity;

        public ProductAdapter(Activity context, List<Product> items)
        {
            this.context = context;
            baseItems = this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Product item = items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.PriceElements[0].name}: {item.PriceElements[0].price}";
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageDrawable(AdapterShared.DLImage(context,item.image));
            return view;
        }

        public override int Count => items.Count;

        public override Product this[int position] => items[position];

        public void SetListType(string type)
        {
            items = baseItems.Where(prd => prd.category == type).ToList();
        }

        public List<string> GetSections()
        {
            List<string> sections = new List<string>();

            foreach (var item in items)
            {
                if (!sections.Any(x => x.Contains(item.section)))
                sections.Add(item.section);
            }
            return sections;
        }
        
        public void SetList(string section)
        {
            items = baseItems.Where(prd => prd.section == section).ToList();
            NotifyDataSetChanged();
        }
    }
}