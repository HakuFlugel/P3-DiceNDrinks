using Android.App;
using Android.Content;
using Android.Content.PM;
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
        private ImageView fbButton;
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
            fbButton = view.FindViewById<ImageView>(Resource.Id.fbImageButton);

            //string[] tempstr = _item.description.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < 100 && i < _item.description.Length; i++) {
                sb.Append(_item.description[i]);
            }
            
            if (_item.description.Length < 100) {
                _expandButton.Visibility = ViewStates.Gone;
            }
            else {
                sb.Append("...");
                _expandButton.Click += setDescribtion_Click;
            }

            _describtiveText.Text = sb.ToString();


            fbButton.Click += (s, e) => {
                StartActivity(newFacebookIntent(Android.App.Application.Context.PackageManager, "https://www.facebook.com/events/" + _item.facebookID));
            };
            

            return view;/*base.OnCreateView(inflater, container, savedInstanceState);*/
        }

        private void setDescribtion_Click(object sender, EventArgs e) {
            _describtiveText.Text = _item.description;
            _expandButton.Text = "Less info";
            _expandButton.Click -= setDescribtion_Click;
            _expandButton.Click += DeSetDescribtion_Click;
        }

        private void DeSetDescribtion_Click(object sender, EventArgs e) {

            _describtiveText.Text = sb.ToString();
            _expandButton.Text = "More info";
            _expandButton.Click += setDescribtion_Click;
        }

        public static Intent newFacebookIntent(PackageManager pm, String url) {
            Android.Net.Uri uri = Android.Net.Uri.Parse(url);
            try {
                ApplicationInfo applicationInfo = pm.GetApplicationInfo("com.facebook.katana", 0);
                if (applicationInfo.Enabled) {
                    // http://stackoverflow.com/a/24547437/1048340
                    uri = Android.Net.Uri.Parse("fb://facewebmodal/f?href=" + url);
                }
            }
            catch (PackageManager.NameNotFoundException ignored) {
            }
            return new Intent(Intent.ActionView, uri);
        }
    }
}