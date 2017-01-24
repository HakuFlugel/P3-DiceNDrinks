using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidAppV2.ListAdapters;

using Shared;
using AndroidAppV2.ListDialogFragments;

namespace AndroidAppV2.Activities {
    [Activity(Label = "Events", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EventActivity : Activity {
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.eventLayout);

            ListView listView = FindViewById<ListView>(Resource.Id.listView1);

            List<Event> list;
            AndroidShared.LoadData(this, "events.json", out list);


            EventAdapter itemAdapter = new EventAdapter(this, list);
            listView.Adapter = itemAdapter;
            listView.ItemClick += (s, e) => {
                EventDialogFragment dialog = new EventDialogFragment(list[e.Position]);
                dialog.Show(FragmentManager, "Event Dialog");
            };
        }

        protected override void OnResume() {
            base.OnResume();
            AndroidShared.Update();
        }
    }
}
