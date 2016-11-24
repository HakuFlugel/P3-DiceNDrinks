using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Shared;

namespace AndroidAppV2.ListDialogFragments
{
    public class EventDialogFragment : DialogFragment
    {
        private readonly Event _item;

        public EventDialogFragment(Event item)
        {
            _item = item;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //NO TITLE
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.eventDialogView, container, true);

            view.FindViewById<TextView>(Resource.Id.textViewTitle).Text = _item.name;
            view.FindViewById<TextView>(Resource.Id.textViewDateTime).Text = _item.startDate.ToShortDateString() + " " + _item.startDate.ToShortTimeString() + " - " + _item.endDate.ToShortTimeString();
            view.FindViewById<TextView>(Resource.Id.textViewDescription).Text = _item.description;

            return view;/*base.OnCreateView(inflater, container, savedInstanceState);*/
        }
    }
}