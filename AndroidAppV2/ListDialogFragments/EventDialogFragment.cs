using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
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
        private Button _expandButton;
        private TextView _describtiveText;
        private ImageView _fbButton;
        private readonly StringBuilder _sb = new StringBuilder();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.eventDialogView, container, true);

            view.FindViewById<TextView>(Resource.Id.textViewTitle).Text = _item.name;
            view.FindViewById<TextView>(Resource.Id.textViewDateTime).Text = _item.startDate.ToShortDateString() + " " + _item.startDate.ToShortTimeString() + " - " + _item.endDate.ToShortTimeString();
            

            _expandButton = view.FindViewById<Button>(Resource.Id.expandButton);
            _describtiveText = view.FindViewById<TextView>(Resource.Id.textViewDescription);
            _fbButton = view.FindViewById<ImageView>(Resource.Id.fbImageButton);

            for (int i = 0; i < 100 && i < _item.description.Length; i++) {
                _sb.Append(_item.description[i]);
            }
            
            if (_item.description.Length < 100) {
                _expandButton.Visibility = ViewStates.Gone;
            }
            else {
                _sb.Append("...");
                _expandButton.Click += setDescribtion_Click;
            }

            _describtiveText.Text = _sb.ToString();


            _fbButton.Click += (s, e) => {
                StartActivity(NewFacebookIntent(Application.Context.PackageManager, "https://www.facebook.com/events/" + _item.facebookID));
            };
            

            return view;
        }

        private void setDescribtion_Click(object sender, EventArgs e) {
            _describtiveText.Text = _item.description;
            _expandButton.Text = "Less info";
            _expandButton.Click -= setDescribtion_Click;
            _expandButton.Click += DeSetDescribtion_Click;
        }

        private void DeSetDescribtion_Click(object sender, EventArgs e) {

            _describtiveText.Text = _sb.ToString();
            _expandButton.Text = "More info";
            _expandButton.Click += setDescribtion_Click;
        }

        public static Intent NewFacebookIntent(PackageManager pm, string url) {
            Android.Net.Uri uri = Android.Net.Uri.Parse(url);
            try {
                ApplicationInfo applicationInfo = pm.GetApplicationInfo("com.facebook.katana", 0);
                if (applicationInfo.Enabled) {
                    uri = Android.Net.Uri.Parse("fb://facewebmodal/f?href=" + url);
                }
            }
            catch (PackageManager.NameNotFoundException) {
            }
            return new Intent(Intent.ActionView, uri);
        }
    }
}