using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

using Shared;
using Java.Lang;

namespace AndroidAppV2.ListAdapters
{
    internal class GameAdapter : BaseAdapter<Game>
    {
        private List<Game> _items;
        private readonly List<Game> _baseItems;
        private readonly Activity _context;


        public GameAdapter(Activity context, List<Game> items)
        {
            _context = context;
            _baseItems = _items = items;
            Sort("Alfabetisk");
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Game item = _items[position];
            //sets the view as convertView unless convertView is null
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.gameItemView, null);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < item.genre.Count && i < 3; i++) {
                sb.Append(item.genre[i] + ",  ");
            }

            view.FindViewById<TextView>(Resource.Id.gameGenreText).Text = sb.ToString(); //chooses the first because genre is a list q.q
            view.FindViewById<TextView>(Resource.Id.gameNameText).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.gameTimeText).Text = "Time: " + item.minPlayTime + "-" + item.maxPlayTime;
            view.FindViewById<TextView>(Resource.Id.gamePlayerText).Text = "Players: " + item.minPlayers + "-" + item.maxPlayers;

            return view;
        }

        public override int Count => _items.Count;

        public override Game this[int position] => _items[position];

        //collects garbage and updates the listview
        private void RemoveGarbage()
        {
            NotifyDataSetChanged();
            GC.Collect();
        }

        //switches the list between being ascending and descending
        public void SwitchOrder()
        {
            _items.Reverse();
            NotifyDataSetChanged();
            GC.Collect();
        }

        //Spinner sorter
        public void Sort(string key)
        {
            switch (key)
            {
                case "Alfabetisk":
                    _items.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                    break;
                case "Max. Spillere":
                    _items.Sort((a,b) => b.maxPlayers.CompareTo(a.maxPlayers));
                    break;
                case "Min. Spillere":
                    _items.Sort((a, b) => a.minPlayers.CompareTo(b.minPlayers));
                    break;
                case "Max. Spilletid":
                    _items.Sort((a, b) => b.maxPlayTime.CompareTo(a.maxPlayTime));
                    break;
                case "Min. Spilletid":
                    _items.Sort((a, b) => a.minPlayTime.CompareTo(b.minPlayTime));
                    break;
                case "Sv�rhedsgrad":
                    _items.Sort((a,b) => a.difficulity.CompareTo(b.difficulity));
                    break;
                case "Udgivelses�r":
                    _items.Sort((a,b) => a.publishedYear.CompareTo(b.publishedYear));
                    break;
                default:
                    throw new KeyNotFoundException($"Could not find key: \"{key}\"");
            }
            NotifyDataSetChanged();
            GC.Collect();
        }

        //Gets the game according to the position in the listview
        public Game GetGameByPosition(int pos)
        {
            return _items[pos];
        }

        //search name
        public void NameSearch(string value)
        {
            List<Game> searchList = new List<Game>();
            foreach (Game game in _baseItems.ToList())
            {
                if (game.name.ToLower().Contains(value.ToLower()) && !searchList.Contains(game))
                    searchList.Add(game);
                else if (searchList.Contains(game))
                    searchList.Remove(game);
            }
            _items = searchList;
            RemoveGarbage();
            NotifyDataSetChanged();
        }

        //search genre(s) //TODO: Implement more search
        /*public void AdvancedSearch(string[] value)
        {
            foreach (Game game in _baseItems)
            {
                if (game.genre.Any(x => value.Any(y => y.ToString() == x)) && !_items.Contains(game))
                    _items.Add(game);
                else if (_items.Contains(game))
                    _items.Remove(game);
            }

            RemoveGarbage();
        }

        //search num. of players, difficulty, and playtime 
        public void AdvancedSearch(string item, int value)
        {
            switch (item)
            {
                case "MaxPlayers":
                    foreach (Game game in _items)
                    {
                        if (game.maxPlayers > value && !_items.Contains(game))
                        {
                            _items.Add(game);
                        }
                        else if (_items.Contains(game))
                        {
                            _items.Remove(game);
                        }
                    }
                    break;
                case "MinPlayers":
                    foreach (Game game in _items)
                    {
                        if (game.minPlayers > value && !_items.Contains(game))
                        {
                            _items.Add(game);
                        }
                        else if (_items.Contains(game))
                        {
                            _items.Remove(game);
                        }
                    }
                    break;
                case "MinDiff":
                    foreach (Game game in _items)
                    {
                        if (game.difficulity > value && !_items.Contains(game))
                        {
                            _items.Add(game);
                        }
                        else if (_items.Contains(game))
                        {
                            _items.Remove(game);
                        }
                    }
                    break;
            }
            RemoveGarbage();
        }*/

        public void ResetSearch()
        {
            _items = _baseItems;
            RemoveGarbage();
        }
    }
}