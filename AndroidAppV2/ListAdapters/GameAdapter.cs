using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using AndroidAppV2.Activities;
using Shared;

namespace AndroidAppV2.ListAdapters
{
    class GameAdapter : BaseAdapter<Game>
    {
        List<Game> items;
        Activity context;
        private FoodmenuActivity foodmenuActivity;
        private bool _ascending = true;

        public GameAdapter(Activity context, List<Game> items)
        {
            this.context = context;
            this.items = items;
            Sort("Alphabetical");
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
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.genre[0]}"; //chooses the first because genre apperently is a list q.q
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(AdapterShared.getBitmapFromAsset(context,item.thumbnail)); //SetImageDrawable(AdapterShared.DLImage(context, item.thumbnail));
            return view;
        }

        public override int Count => items.Count;

        public override Game this[int position] => items[position];

        public void SwitchOrder()
        {
            if (_ascending)
                _ascending = false;
            else
                _ascending = true;

            items.Reverse();
            NotifyDataSetChanged();
            GC.Collect();
        }

        public void Sort(string key)
        {
            switch (key)
            {
                case "Alphabetical":
                    items.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                    break;
                case "Max. Players":
                    items.Sort((a,b) => a.maxPlayers.CompareTo(b.maxPlayers));
                    break;
                case "Min. Players":
                    items.Sort((a, b) => a.minPlayers.CompareTo(b.minPlayers));
                    break;
                case "Max. Game Time":
                    items.Sort((a, b) => a.maxPlayTime.CompareTo(b.maxPlayTime));
                    break;
                case "Min. Game Time":
                    items.Sort((a, b) => a.minPlayTime.CompareTo(b.minPlayTime));
                    break;
                case "Difficulty":
                    items.Sort((a,b) => a.difficulity.CompareTo(b.difficulity));
                    break;
                case "Year of publication":
                    items.Sort((a,b) => a.publishedYear.CompareTo(b.publishedYear));
                    break;
                default:
                    new KeyNotFoundException($"Could not find key: {key}");
                    break;
            }
            NotifyDataSetChanged();
            GC.Collect();
        }

    }
}