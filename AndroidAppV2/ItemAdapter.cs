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
using AndroidAppV2.Activities;
using Shared;

namespace AndroidAppV2
{
    class ItemAdapter : BaseAdapter<Product>
    {
        List<Product> items;
        Activity context;
        private FoodmenuActivity foodmenuActivity;

        public ItemAdapter(Activity context, List<Product> items)
       : base()
   {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = item.category; //todo: pris
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Int32.Parse(item.image)); //todo: skal sikres at det kan parses
            return view;
        }

        public override int Count => items.Count;

        public override Product this[int position] => items[position];
    }
}