﻿using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;



namespace AndroidAppV2.Activities {
    [Activity(Label = "Games", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GameActivity : Activity {
        private bool _ascending = true;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameLayout);


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

            gameSpinner.ItemSelected += delegate {
                try {
                    itemAdapter.Sort(gameSpinner.SelectedItemPosition);
                }
                catch (KeyNotFoundException e) {
                    Log.WriteLine(LogPriority.Error, $"X:{this}", e.Message);
                }

                if (_ascending)
                    return;

                itemAdapter.SwitchOrder();
                _ascending = true;
            };

            gameSearch.TextChanged += (s, e) => itemAdapter.NameSearch(gameSearch.Text);


            gameButton.Click += delegate {
                itemAdapter.SwitchOrder();
                SwitchAscending();
            };

            listView.Adapter = itemAdapter;
            listView.ItemClick += (s, e) => {
                Game theGame = itemAdapter.GetGameByPosition(e.Position);

                GameDialogFragment dialog = new GameDialogFragment();
                dialog.PassDataToFrag(theGame);
                dialog.Show(FragmentManager, "Game Dialog");
            };
        }

        private void SwitchAscending() {
            Button gameButton = FindViewById<Button>(Resource.Id.gameSortOrderButton);

            if (_ascending) {
                _ascending = false;
                gameButton.Text = "↑";
                return;
            }
            _ascending = true;
            gameButton.Text = "↓";
        }


        private List<Game> GetGames() {
            List<Game> list;

            AndroidShared.LoadData(this, "games.json", out list);

            return list;
        }

        protected override void OnResume() {
            base.OnResume();
            AndroidShared.Update();
        }

    }
}