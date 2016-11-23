using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;

using AndroidAppV2.Activities;
using Shared;
using Enumerable = System.Linq.Enumerable;

namespace AndroidAppV2.ListAdapters
{
    internal class GameAdapter : BaseAdapter<Game>
    {
        private List<Game> _items;
        private List<Game> _baseItems;
        private readonly Activity _context;


        public GameAdapter(Activity context, List<Game> items)
        {
            _context = context;
            _baseItems = _items = items;
            Sort("Alphabetical");
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Game item = _items[position];
            //sets the view as convertView unless convertView is null
            AndroidShared an = new AndroidShared();
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.CustomItemView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.genre[0]}"; //chooses the first because genre apperently is a list q.q
            int[] sizes = {75, 75};
            an.GetImages(_context, $"ProductPics/{item.thumbnail}.png", view, Resource.Id.productImage, sizes);
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(AndroidShared.GetBitmapFromAsset(_context,item.thumbnail)); //SetImageDrawable(AdapterShared.DLImage(context, item.thumbnail));
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
                case "Alphabetical":
                    _items.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                    break;
                case "Max. Players":
                    _items.Sort((a,b) => a.maxPlayers.CompareTo(b.maxPlayers));
                    break;
                case "Min. Players":
                    _items.Sort((a, b) => a.minPlayers.CompareTo(b.minPlayers));
                    break;
                case "Max. Game Time":
                    _items.Sort((a, b) => a.maxPlayTime.CompareTo(b.maxPlayTime));
                    break;
                case "Min. Game Time":
                    _items.Sort((a, b) => a.minPlayTime.CompareTo(b.minPlayTime));
                    break;
                case "Difficulty":
                    _items.Sort((a,b) => a.difficulity.CompareTo(b.difficulity));
                    break;
                case "Year of Publication":
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
        public void AdvancedSearch(string value)
        {
            foreach (Game game in _baseItems)
            {
                if (game.name.Contains(value) && !_items.Contains(game))
                    _items.Add(game);
                else if (_items.Contains(game))
                    _items.Remove(game);
            }

            RemoveGarbage();
        }

        //search genre(s)
        public void AdvancedSearch(string[] value)
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

        //search num. of players, difficulty, and playtime //TODO: Implement more search
        /*public void AdvancedSearch(string item, int value)
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