using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace AndroidAppV2.ListDialogFragments {
    public class GameDialogFragment : Android.Support.V4.App.DialogFragment {

        Button Button_Dismiss;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle bundle) {

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            var view = inflater.Inflate(Resource.Layout.GameDialogView, container, true);

            //Button test dismiss
            Button_Dismiss = view.FindViewById<Button>(Resource.Id.Button_Dismiss);
            Button_Dismiss.Click += Button_Dismiss_Click;

            return view;
        }

        public override void OnResume() {

            // Auto Size based on content
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // No background behind the view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Red));

            //Disables standard dialog style/frame/theme and to make it look like full UI
            SetStyle(Android.Support.V4.App.DialogFragment.StyleNoFrame, Android.Resource.Style.Theme);

            base.OnResume();
        }
        private void Button_Dismiss_Click(object sender, EventArgs e) {
            Dismiss();
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing) {
                Button_Dismiss.Click -= Button_Dismiss_Click;
            }
        }
    }
}