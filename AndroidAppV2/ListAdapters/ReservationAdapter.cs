/*
using System.Collections.Generic;

using Android.App;

using Android.Views;
using Android.Widget;

using Shared;

namespace AndroidAppV2.ListAdapters
{
    class ReservationAdapter : BaseAdapter<Event>
    {
        private readonly List<Event> _items;
        private readonly Activity _context;

        public ReservationAdapter(Activity context, List<Event> items)
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Event item = _items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.eventListItem, null);
            view.FindViewById<TextView>(Resource.Id.eventNameText).Text = item.name;
            return view;
        }

        public override int Count => _items.Count;

        public override Event this[int position] => _items[position];

    }
}*/