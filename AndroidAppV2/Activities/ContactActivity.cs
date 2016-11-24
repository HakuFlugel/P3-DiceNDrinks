

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using Android.Widget;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Contact", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ContactActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.contactLayout);

            // Create your application here

            //TODO: Vi skal ikke rigtig lave noget vildt her... kun noget tekst
            //https://developer.xamarin.com/recipes/android/fundamentals/intent/launch_the_map_application/
            Button button = FindViewById<Button>(Resource.Id.MapButton);
            Android.Net.Uri geoUri = Android.Net.Uri.Parse("geo:57.046581, 9.916551?z=18");
            Intent mapIntent = new Intent(Intent.ActionView, geoUri);

            button.Click += delegate
            {
                StartActivity(mapIntent);
            };
        }
    }
}