using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class GamesController : ControllerBase
    {
        public List<Game> games = new List<Game>();
        public List<string> genres = new List<string>();

        public GamesController(string path = "data/") : base(path)
        {
        }

        public event EventHandler GameUpdated;

        public void addGame(Game newGame)
        {
            newGame.id = new Random().Next(); // TODO: unique id

            games.Add(newGame);

            GameUpdated?.Invoke(this, EventArgs.Empty);
        }

        private Random rand = new Random();

        private int getRandomID()
        {
            int id;

            do id = rand.Next(); while (games.Exists(game => game.id == id));

            return id;

        }


        public void updateGame(Game newGame)
        {
            Game oldGame = games.First(g => g.id == newGame.id);

            if (oldGame == null)
            {
                return;
            }

            games[games.FindIndex(e => e == oldGame)] = newGame;

            GameUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeGame(Game oldGame)
        {

            games.Remove(oldGame); //TODO: make sure this uses id to compare

            GameUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void addGenre(string genre)
        {
            genres.Add(genre);
        }

        public void removeGenre(string genre)
        {
            genres.Remove(genre);

            foreach (var game in games)
            {
                game.genres.Remove(genre);
            }
        }

        public void renameGenre(string oldgenre, string genre)
        {


            foreach (var game in games)
            {
                int index = game.genres.FindIndex(g => g == oldgenre);
                if (index >= 0)
                {
                    game.genres[index] = genre;
                }
            }
        }
//
//        public void renameGenre()
//        {
//
//        }

        public override void save()
        {
            saveFile("games", games);
            saveFile("genres", genres);
        }

        public override void load()
        {
            games = loadFile<Game>("games");
            genres = loadFile<string>("genres");
        }
    }
}