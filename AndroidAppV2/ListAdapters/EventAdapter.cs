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
    class EventAdapter : BaseAdapter<Event>
    {
        List<Event> items;
        Activity context;

        public EventAdapter(Activity context, List<Event> items)
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
            Event item = items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.eventListItem, null);
            view.FindViewById<TextView>(Resource.Id.eventNameText).Text = item.name;//TODO Fucking naming....
            //view.FindViewById<TextView>(Resource.Id.Text2).Text = item.description;
            view.FindViewById<TextView>(Resource.Id.eventDateText).Text = item.startDate.ToShortDateString();
            view.FindViewById<TextView>(Resource.Id.eventTimeText).Text = item.startDate.ToShortTimeString() + "-" + item.endDate.ToShortTimeString();
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(AdapterShared.getBitmapFromAsset(context, item.image)); // TODO: not sure if we need a thumbnail
            return view;
        }

        public override int Count => items.Count;

        public override Event this[int position] => items[position];

    }
}