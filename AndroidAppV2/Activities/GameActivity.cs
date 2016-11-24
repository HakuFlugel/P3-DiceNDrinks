﻿using System;
using System.Collections.Generic;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Java.Lang;
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GameActivity : FragmentActivity
    {

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

            gameSpinner.ItemSelected += delegate {
                itemAdapter.Sort((string)gameSpinner.SelectedItem);
            };

            gameSearch.AddTextChangedListener(new TextWatcher());


            gameButton.Click += delegate
            {
                gameButton.Text = gameButton.Text == "↓" ? "↑" : "↓";

                itemAdapter.SwitchOrder();
            };


            listView.Adapter = itemAdapter;
            listView.ItemClick += (s,e) => {
                Game theGame = itemAdapter.GetGameByPosition(e.Position);
                
                var dialog = new GameDialogFragment();
                dialog.PassDataToFrag(theGame);
                dialog.Show(FragmentManager, "Game Dialog");
            };
        }



        List<Game> GetGames()
        {
            List<Game> list;

            AndroidShared.LoadData(this,"games.json",out list);

            return list;
        }

    }

    internal class TextWatcher : ITextWatcher {
        public IntPtr Handle {
            get {
                throw new NotImplementedException();
            }
        }

        public void AfterTextChanged(IEditable s) {
            
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after) {
            
        }

        public void Dispose() {
            return;
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count) {
            
        }
    }
}