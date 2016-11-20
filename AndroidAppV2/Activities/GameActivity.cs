using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;

using AndroidAppV2.ListAdapters;
using AndroidAppV2.ListDialogFragments;
using Shared;


namespace AndroidAppV2.Activities
{
    [Activity(Theme = "@style/Theme.NoTitle", Label = "Games")]
    public class GameActivity : FragmentActivity
    {
        public static Game GAME_FRAGMENT_KEY;

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
            GameAdapter itemAdapter = new GameAdapter(this, list);
            listView.Adapter = itemAdapter;

            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.gameSortArray, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            gameSpinner.Adapter = adapter;

            gameSpinner.ItemSelected += delegate
            {
                itemAdapter.Sort((string)gameSpinner.SelectedItem);
            };


            gameButton.Click += delegate
            {
                gameButton.Text = gameButton.Text == "↓" ? "↑" : "↓";

                itemAdapter.SwitchOrder();
            };

            aSearchButton.Click += delegate
            {
                //TODO: Make Search Limit Fragment
            };

            


            listView.Adapter = itemAdapter;
            listView.ItemClick += OnListItemClick;
        }


        List<Game> getGames()
        {
            List<Game> list = new List<Game>();


            Game testgame = new Game
            {
                name = "Secret Hitler",
                genre = { "Party game"},
                thumbnail = "small.jpg", //TODO: this is a placeholder, both the file-path and the method to retrieve it
                description = "Gas(Guess) who",
                difficulity = 99,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 20,
                maxPlayTime = 60,
                publishedYear = 2015
            };
            Game testgame2 = new Game
            {
                name = "In time you will know the tragic extends of my failings",
                genre = {"how","quickly","the","tide","turns"},
                thumbnail = "small.jpg",
                difficulity = 100,
                minPlayers = 1,
                maxPlayers = 1,
                minPlayTime = 2,
                maxPlayTime = 1000,
                publishedYear = 2014

            };
            Game testgame3 = new Game
            {
                name = "wawaawawawawwawa",
                genre = { "WALUIGI" },
                thumbnail = "small.jpg",
                difficulity = 05,
                minPlayers = 1,
                maxPlayers = 2,
                minPlayTime = 60,
                maxPlayTime = 61,
                publishedYear = 2005
            };
            Game testgame4 = new Game
            {
                name = "uguu~",
                genre = { "fucking degenerate" },
                thumbnail = "small.jpg",
                difficulity = 1,
                minPlayers = 2,
                maxPlayers = 8,
                minPlayTime = 20,
                maxPlayTime = 110,
                publishedYear = 1945
            };
            Game testgame5 = new Game
            {
                name = "Look at this net",
                genre = { "that i just found" },
                thumbnail = "small.jpg",
                difficulity = 50,
                minPlayers = 3,
                maxPlayers = 4,
                minPlayTime = 2,
                maxPlayTime = 5,
                publishedYear = 2016
            };
            Game testgame6 = new Game
            {
                name = "CUT MY LIFE INTO PIECES",
                genre = { "THIS IS MY LAST", "burrito" },
                thumbnail = "small.jpg",
                difficulity = 75,
                minPlayers = 4,
                maxPlayers = 9,
                minPlayTime = 15,
                maxPlayTime = 60,
                publishedYear = 2007
            };
            Game testgame7 = new Game
            {
                name = "You tell them you're fine",
                genre = { "but really you're not fine" },
                thumbnail = "small.jpg",
                difficulity = 88,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 20,
                maxPlayTime = 60,
                publishedYear = 2000
            };
            Game testgame8 = new Game
            {
                name = "Rape",
                genre = { "Family" },
                description = "Hemcest ;)",
                thumbnail = "small.jpg",
                difficulity = 69,
                minPlayers = 2,
                maxPlayers = 4,
                minPlayTime = 2,
                maxPlayTime = 5,
                publishedYear = 1996
            };
            Game testgame9 = new Game
            {
                name = "Fordi du løber tør for tid~",
                genre = { "WHAT" },
                thumbnail = "small.jpg",
                difficulity = 51,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 20,
                maxPlayTime = 60,
                publishedYear = 1990
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
            Game theGame = getGames()[e.Position];
            var listView = sender as ListView;
            //todo: dialog fragment med yderligere info om et item
            var dialog = new GameDialogFragment();
            //dialog.Show(FragmentManager, "lel");
            dialog.PassDataToFrag(theGame);
            dialog.Show(FragmentManager, "Game Dialog");
        }


    }
}