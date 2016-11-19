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

namespace AndroidAppV2
{
    class GameAdapter : BaseAdapter<Game>
    {
        List<Game> items;
        Activity context;
        private FoodmenuActivity foodmenuActivity;

        public GameAdapter(Activity context, List<Game> items)
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
            Game item = items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.publishedYear}";
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageDrawable(DLImage(item.thumbnail));
            return view;
        }

        public override int Count => items.Count;

        public override Game this[int position] => items[position];

        Drawable DLImage(string path)
        {

            Stream asset = context.Assets.Open(path);

            Drawable d = Drawable.CreateFromStream(asset,null);

            return d;
        }

    }
}