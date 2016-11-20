using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Shared;

namespace AndroidAppV2.ListDialogFragments
{
    public class EventDialogFragment : DialogFragment
    {
        private Event item;

        public EventDialogFragment(Event item)
        {
            this.item = item;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.eventDialogView, container, true);

            view.FindViewById<TextView>(Resource.Id.textViewTitle).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.textViewDateTime).Text = item.startDate.ToShortDateString() + " " + item.startDate.ToShortTimeString() + " - " + item.endDate.ToShortTimeString();
            view.FindViewById<TextView>(Resource.Id.textViewDescription).Text = item.description;

            return view;/*base.OnCreateView(inflater, container, savedInstanceState);*/
        }
    }
}