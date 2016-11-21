using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shared;

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
    public class GameDialogFragment : DialogFragment {

        private Button Button_Dismiss;
        private TextView Text_something;
        private Game game;

        public override View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle bundle) {
            
            //Maybe unnnesscarydyryryryr
            //Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            var view = inflater.Inflate(Resource.Layout.GameDialogView, container, true);

            StringBuilder sb = new StringBuilder();

            foreach (var item in game.genre) {
                sb.Append(item + ",  ");
            }
            sb.Remove(sb.Length - 3, 3);

            //Test textssssss
            view.FindViewById<TextView>(Resource.Id.gameNameText).Text = game.name;
            view.FindViewById<TextView>(Resource.Id.gameDescrText).Text = game.description;
            view.FindViewById<TextView>(Resource.Id.gamePlayerText).Text = "Players: " + game.minPlayers.ToString() + "-" + game.maxPlayers.ToString();
            view.FindViewById<TextView>(Resource.Id.gamePlayTimeText).Text = "Time: " + game.minPlayTime + "-" + game.maxPlayTime + " min";
            view.FindViewById<TextView>(Resource.Id.gameGenreText).Text = "Genres: " + sb.ToString();
            view.FindViewById<TextView>(Resource.Id.gameDiffText).Text = "Diffuclity: " + game.difficulity.ToString() + "/100";

            //Button test dismiss
            Button_Dismiss = view.FindViewById<Button>(Resource.Id.Button_Dismiss);
            Button_Dismiss.Click += Button_Dismiss_Click;

            return view;
        }
        public void PassDataToFrag(Game game) {
            this.game = game;
        }
        
        public override void OnResume() {

            // Auto Size based on content
            //Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // No background behind the view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.DimGray));

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