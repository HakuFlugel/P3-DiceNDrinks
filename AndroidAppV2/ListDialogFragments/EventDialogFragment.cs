using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Shared;
using System;
using System.Text;

namespace AndroidAppV2.ListDialogFragments
{
    public class EventDialogFragment : DialogFragment
    {
        private readonly Event _item;

        public EventDialogFragment(Event item)
        {
            _item = item;
        }
        private Button expandButton;
        private TextView describtiveText;
        private StringBuilder sb = new StringBuilder();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //NO TITLE
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = inflater.Inflate(Resource.Layout.eventDialogView, container, true);

            view.FindViewById<TextView>(Resource.Id.textViewTitle).Text = _item.name;
            view.FindViewById<TextView>(Resource.Id.textViewDateTime).Text = _item.startDate.ToShortDateString() + " " + _item.startDate.ToShortTimeString() + " - " + _item.endDate.ToShortTimeString();

            expandButton = view.FindViewById<Button>(Resource.Id.expandButton);
            describtiveText = view.FindViewById<TextView>(Resource.Id.textViewDescription);

            
            //string[] tempstr = _item.description.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < 100 && i < _item.description.Length; i++) {
                sb.Append(_item.description[i]);
            }
            
            if (_item.description.Length < 100) {
                expandButton.Visibility = ViewStates.Gone;
            }
            else {
                sb.Append("...");
                expandButton.Click += setDescribtion_Click;
            }

            describtiveText.Text = sb.ToString();

            
            

            return view;/*base.OnCreateView(inflater, container, savedInstanceState);*/
        }

        private void setDescribtion_Click(object sender, EventArgs e) {
            describtiveText.Text = _item.description;
            expandButton.Text = "Less info";
            expandButton.Click -= setDescribtion_Click;
            expandButton.Click += DeSetDescribtion_Click;
        }

        private void DeSetDescribtion_Click(object sender, EventArgs e) {

            describtiveText.Text = sb.ToString();
            expandButton.Text = "More info";
            expandButton.Click += setDescribtion_Click;
        }
    }
}