using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidAppV2.ListAdapters;

using Shared;
using Android.Support.V4.App;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Events", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EventActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.eventLayout);

            // Create your application here

            //TODO: Her skal der laves link til Facebook med FB SDK...
            //https://components.xamarin.com/gettingstarted/facebook-sdk
            //TODO: Eller hvis det bliver et no-go, så have et link til DnD's fb side

            ListView listView = FindViewById<ListView>(Resource.Id.listView1);

            List<Event> list = getEventsTemp();


            EventAdapter itemAdapter = new EventAdapter(this, list);
            listView.Adapter = itemAdapter;
            listView.ItemClick += (s, e) =>
            {
                var dialog = new ListDialogFragments.EventDialogFragment(list[e.Position]);
                dialog.Show(FragmentManager, "dialog");
                //dialog.Show(Android.Support.V4.App.FragmentManager, "derperperpepreprp");

            };
        }

        List<Event> getEventsTemp()
        {
            List<Event> list = new List<Event>();

            AndroidShared.LoadData(this, "events.json", out list);

            /*Event testevent = new Event
            {
                name = "Vaffeldag",
                description = "Massere af vaffler hele dagen",
                //image = "small.jpg" //TODO: this is a placeholder, both the file and the method to retrieve it
            };
            Event testevent2 = new Event
            {
                name = "Mød en ny ven",
                description = "Kom og få en ven, som du kan spille med",
                //image = "small.jpg"
            };


            list.Add(testevent);
            list.Add(testevent2);
            list.Add(testevent2);

            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);
            list.Add(testevent2);*/

            return list;
        }

    }
}
