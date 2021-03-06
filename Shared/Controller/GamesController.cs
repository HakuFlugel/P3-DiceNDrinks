﻿using System;
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
            newGame.id = new Random().Next();

            games.Add(newGame);

            save();
            GameUpdated?.Invoke(this, EventArgs.Empty);
        }


        public void updateGame(Game newGame)
        {
            Game oldGame = games.FirstOrDefault(g => g.id == newGame.id);

            if (oldGame == null)
            {
                throw new Exception("Could not find game");
            }

            games[games.FindIndex(e => e == oldGame)] = newGame;

            save();
            GameUpdated?.Invoke(this, EventArgs.Empty);

        }

        public void removeGame(Game oldGame)
        {

            games.RemoveAll(g => g.id == oldGame.id);
            //games.Remove(oldGame);

            save();
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