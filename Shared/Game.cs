using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class Game
    {
        public Game() { }
        public Game(Game game) { //Copy constructor
            bggid = game.bggid;
            name = game.name;
            description = game.description;
            foreach (var item in game.genre)
                genre.Add(item);
            difficulity = game.difficulity;
            publishedYear = game.publishedYear;
            minPlayers = game.minPlayers;
            maxPlayers = game.maxPlayers;
            minPlayTime = game.minPlayTime;
            maxPlayers = game.maxPlayers;
            thumbnail = game.thumbnail;
            image = game.image;
        }

        public int id;
        public string bggid;

        public string name;
        public string description;
        public List<string> genre;
        public int difficulity;
        public int publishedYear;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail, image;

        
    }
}