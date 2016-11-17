﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Shared;

namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games")]
    public class GameActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.GameLayout);
            // Create your application here



            ListView listView = FindViewById<ListView>(Resource.Id.gameListView);
            Spinner gameSpinner = FindViewById<Spinner>(Resource.Id.gameSpinner);
            Button gameButton = FindViewById<Button>(Resource.Id.gameSortOrderButton);
            Button aSearchButton = FindViewById<Button>(Resource.Id.advancedSearchButton);

            gameSpinner.ItemSelected += spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.gameSortArray, Android.Resource.Layout.SimpleSpinnerItem);

            gameButton.Text = "↓";
            gameButton.Click += delegate
            {
                gameButton.Text = gameButton.Text == "↓" ? "↑" : "↓";

                //TODO: change orientation of search (ascending/descending)
            };

            aSearchButton.Click += delegate
            {
                //TODO: Make Search Limit Fragment
            };

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            gameSpinner.Adapter = adapter;

            List<Game> list = getGames();
            ItemAdapter itemAdapter = new ItemAdapter(this, list);
            listView.Adapter = itemAdapter;

            listView.ItemClick += OnListItemClick;

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            //TODO: skift sorting til valgte element
        }

        List<Game> getGames()
        {
            List<Game> list = new List<Game>();

            //todo: get the games here

            return list;
        }
        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            //todo: dialog fragment med yderligere info om et item
        }
    }
}