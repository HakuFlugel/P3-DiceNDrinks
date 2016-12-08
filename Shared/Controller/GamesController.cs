using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class GamesController : ControllerBase
    {
        public List<Game> games = new List<Game>();

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

        public override void save()
        {
            saveFile("games", games);
        }

        public override void load()
        {
            games = loadFile<Game>("games");
        }
    }
}