using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Events")]
    public class EventActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.eventLayout);

            // Create your application here

            //TODO: Her skal der laves link til Facebook med FB SDK...
            //https://components.xamarin.com/gettingstarted/facebook-sdk
            //TODO: Eller hvis det bliver et no-go, s� have et link til DnD's fb side


        }
    }
}