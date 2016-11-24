
using System.Collections.Generic;

using Android.App;

using Android.Views;
using Android.Widget;

using Shared;

namespace AndroidAppV2.ListAdapters
{
    class EventAdapter : BaseAdapter<Event>
    {
        private readonly List<Event> _items;
        private readonly Activity _context;

        public EventAdapter(Activity context, List<Event> items)
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
            view.FindViewById<TextView>(Resource.Id.eventDateText).Text = item.startDate.ToShortDateString();
            view.FindViewById<TextView>(Resource.Id.eventTimeText).Text = item.startDate.ToShortTimeString() + "-" + item.endDate.ToShortTimeString();
            return view;
        }

        public override int Count => _items.Count;

        public override Event this[int position] => _items[position];

    }
}