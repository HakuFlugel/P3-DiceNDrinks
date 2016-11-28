using System;
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Text;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Java.Lang;
using Shared;
// ReSharper disable All


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GameActivity : FragmentActivity
    {
        private bool _ascending = true;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.GameLayout);
            // Create your application here

            

            ListView listView = FindViewById<ListView>(Resource.Id.gameListView);
            Spinner gameSpinner = FindViewById<Spinner>(Resource.Id.gameSpinner);
            Button gameButton = FindViewById<Button>(Resource.Id.gameSortOrderButton);
            EditText gameSearch = FindViewById<EditText>(Resource.Id.gameSearchEdit);

            List<Game> list = GetGames();
            GameAdapter itemAdapter = new GameAdapter(this, list);
            listView.Adapter = itemAdapter;

            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.gameSortArray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            gameSpinner.Adapter = adapter;

            gameSpinner.ItemSelected += delegate
            {
                try
                {
                    itemAdapter.Sort((string)gameSpinner.SelectedItem);
                }
                catch (KeyNotFoundException e)
                {
                    Log.WriteLine(LogPriority.Error, $"X:{this.ToString()}", e.Message);
                }

                if (!_ascending)
                {
                    itemAdapter.SwitchOrder();
                    _ascending = true;
                }
            };

            gameSearch.TextChanged += (s, e) => {
                itemAdapter.NameSearch(gameSearch.Text);
            };


            gameButton.Click += delegate
            {
                itemAdapter.SwitchOrder();
                SwitchAscending();
            };

            listView.Adapter = itemAdapter;
            listView.ItemClick += (s,e) => {
                Game theGame = itemAdapter.GetGameByPosition(e.Position);
                
                var dialog = new GameDialogFragment();
                dialog.PassDataToFrag(theGame);
                dialog.Show(FragmentManager, "Game Dialog");
            };
        }

        private void SwitchAscending()
        {
            Button gameButton = FindViewById<Button>(Resource.Id.gameSortOrderButton);

            if (_ascending)
            {
                _ascending = false;
                gameButton.Text = "↑";
                return;
            }
            _ascending = true;
            gameButton.Text = "↓";
            return;
        }


        List<Game> GetGames()
        {
            List<Game> list;

            AndroidShared.LoadData(this,"games.json",out list);

            return list;
        }

    }
}