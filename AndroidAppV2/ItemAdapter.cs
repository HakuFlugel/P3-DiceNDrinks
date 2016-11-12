using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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
            Product item = items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.PriceElements[0].price}"; // Viser kun den første pris
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageDrawable(DLImage(item.image));
            return view;
        }

        public override int Count => items.Count;

        public override Product this[int position] => items[position];

        Drawable DLImage(string path)
        {
            return Drawable.CreateFromPath("" + path); //todo: image download path
        }
    }
}