
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

using Android.Support.V7.App;
using Android.Util;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Label = "Dice 'n Drinks", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        // ReSharper disable once InconsistentNaming
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        protected override async void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => {

                 //TODO: Download games/menu here
            });

            startupWork.ContinueWith(t => {
                Log.Debug(TAG, "Work is finished - start MainActivity");
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}