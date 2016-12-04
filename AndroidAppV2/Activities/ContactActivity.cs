using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace AndroidAppV2.Activities
{
    [Activity(Label = "Kontakt Information", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContactActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.contactLayout);

            Button button = FindViewById<Button>(Resource.Id.MapButton);
            TextView phone = FindViewById<TextView>(Resource.Id.AboutUsPhone);
            TextView mail = FindViewById<TextView>(Resource.Id.AboutUsEmail);

            Android.Net.Uri geoUri = Android.Net.Uri.Parse("geo:57.046581, 9.916551?z=18");
            Intent mapIntent = new Intent(Intent.ActionView, geoUri);


            Android.Net.Uri phoneUri = Android.Net.Uri.Parse("tel:+4522767651");
            Intent phoneIntent = new Intent(Intent.ActionDial, phoneUri);

            Intent mailIntent = new Intent(Intent.ActionSend);
            mailIntent.PutExtra(Intent.ExtraEmail, new[] {"info@dicendrinks.com"});
            mailIntent.SetType("message/rfc822");

            phone.Click += delegate
            {
                StartActivity(phoneIntent);
            };

            button.Click += delegate
            {
                StartActivity(mapIntent);
            };

            mail.Click += delegate
            {
                StartActivity(mailIntent);
            };
        }
    }
}