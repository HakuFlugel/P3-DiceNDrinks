using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class Game
    {
        public Game() {

        }
        public Game(Game game) { //Copy constructor
            id = game.id;
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
            imageName = game.imageName;
        }
        public string id = "";

        public string name = "";
        public string description = "";
        public List<string> genre = new List<string>();
        public int difficulity = 0;
        public int publishedYear = 0;

        public int minPlayers = 0, maxPlayers = 0;
        public int minPlayTime = 0, maxPlayTime = 0;

        public string thumbnail = "", imageName = "";

        
    }
}