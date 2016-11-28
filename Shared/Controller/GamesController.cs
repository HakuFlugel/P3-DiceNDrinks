using System;
using System.Collections.Generic;

namespace Shared
{
    public class GamesController : ControllerBase
    {
        List<Game> games = new List<Game>();

        public event EventHandler<UpdateGameEventArgs> GameUpdated;
        public class UpdateGameEventArgs
        {
            public Game newGame;
            public Game oldGame;

            public UpdateGameEventArgs(Game oldGame, Game newGame)
            {
                this.oldGame = oldGame;
                this.newGame = newGame;
            }
        }

        public void addGame(Game newGame)
        {
            newGame.id = new Random().Next(); // TODO: unique id

            games.Add(newGame);

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(null, newGame));
        }


        public void updateGame(Game oldGame, Game newGame)
        {
            //newProduct.created = oldProduct.created;
            newGame.id = oldGame.id; // TODO: needed?

            games[games.FindIndex(e => e == oldGame)] = newGame;

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(oldGame, newGame));

        }

        public void removeGame(Game oldGame)
        {

            games.Remove(oldGame); //TODO: make sure this uses id to compare

            GameUpdated?.Invoke(this, new UpdateGameEventArgs(oldGame, null));
        }

        public override void save()
        {
            saveFile("data/games.json", games);
        }

        public override void load()
        {
            games = loadFile<Game>("data/games.json");
        }
    }
}