using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games")]
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
            Button aSearchButton = FindViewById<Button>(Resource.Id.advancedSearchButton);
            List<Game> list = getGames();
            
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


            GameAdapter itemAdapter = new GameAdapter(this, list);
            listView.Adapter = itemAdapter;
            listView.ItemClick += delegate {
                var dialog = new ListDialogFragments.GameDialogFragment();
                dialog.Show(SupportFragmentManager, "dialog");
                
            };

        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            //TODO: skift sorting til valgte element
        }

        List<Game> getGames()
        {
            List<Game> list = new List<Game>();


            Game testgame = new Game
            {
                name = "Secret Hitler",
                genre = { "Party game"},
                thumbnail = "small.jpg" //TODO: this is a placeholder, both the file-path and the method to retrieve it
            };
            Game testgame2 = new Game
            {
                name = "In time you will know the tragic extends of my failings",
                genre = {"how","quickly","the","tide","turns"},
                thumbnail = "small.jpg"
            };
            Game testgame3 = new Game
            {
                name = "wawaawawawawwawa",
                genre = { "WALUIGI" },
                thumbnail = "small.jpg"
            };
            Game testgame4 = new Game
            {
                name = "uguu~",
                genre = { "fucking degenerate" },
                thumbnail = "small.jpg"
            };
            Game testgame5 = new Game
            {
                name = "Look at this net",
                genre = { "that i just found" },
                thumbnail = "small.jpg"
            };
            Game testgame6 = new Game
            {
                name = "CUT MY LIFE INTO PIECES",
                genre = { "THIS IS MY LAST", "burrito" },
                thumbnail = "small.jpg"
            };
            Game testgame7 = new Game
            {
                name = "You tell them you're fine",
                genre = { "but really you're not fine" },
                thumbnail = "small.jpg"
            };
            Game testgame8 = new Game
            {
                name = "Rape",
                genre = { "Family" },
                description = "Hemcest ;)",
                thumbnail = "small.jpg"
            };
            Game testgame9 = new Game
            {
                name = "Fordi du løber tør for tid~",
                genre = { "WHAT" },
                thumbnail = "small.jpg"
            };


            list.Add(testgame);
            list.Add(testgame2);
            list.Add(testgame3);
            list.Add(testgame4);
            list.Add(testgame5);
            list.Add(testgame6);
            list.Add(testgame7);
            list.Add(testgame8);
            list.Add(testgame9);


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