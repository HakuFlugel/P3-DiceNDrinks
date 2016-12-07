using System;
using System.Text;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;

using Shared;

namespace AndroidAppV2.ListDialogFragments {
    public class GameDialogFragment : DialogFragment {

        private Button _buttonDismiss;
        private Game _game;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle) {

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            //Create view
            var view = inflater.Inflate(Resource.Layout.GameDialogView, container, true);
            
            StringBuilder sb = new StringBuilder();

            foreach (string item in _game.genre) {
                sb.Append(item + ",  ");
            }
            sb.Remove(sb.Length - 3, 3);

            view.FindViewById<TextView>(Resource.Id.gameNameText).Text = _game.name;
            view.FindViewById<TextView>(Resource.Id.gameDescrText).Text = _game.description;
            view.FindViewById<TextView>(Resource.Id.gamePlayerText).Text = "Players: " + _game.minPlayers + "-" + _game.maxPlayers;
            view.FindViewById<TextView>(Resource.Id.gamePlayTimeText).Text = "Time: " + _game.minPlayTime + "-" + _game.maxPlayTime + " min";
            view.FindViewById<TextView>(Resource.Id.gameGenreText).Text = "Genres: " + sb;
            view.FindViewById<TextView>(Resource.Id.gameDiffText).Text = "Diffuclity: " + _game.difficulity + "/10";
            if (_game.bggid != null) {
                view.FindViewById<TextView>(Resource.Id.gameHyperText).Text = "Data provided by BoardGameGeek";
            }
            
            _buttonDismiss = view.FindViewById<Button>(Resource.Id.Button_Dismiss);
            _buttonDismiss.Click += Button_Dismiss_Click;

            return view;
        }
        public void PassDataToFrag(Game game) {
            _game = game;
        }
        
        public override void OnResume() {

            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.DarkGray));

            base.OnResume();
        }
        private void Button_Dismiss_Click(object sender, EventArgs e) {
            Dismiss();
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            
            if (disposing) {
                _buttonDismiss.Click -= Button_Dismiss_Click;
            }
        }
    }
}