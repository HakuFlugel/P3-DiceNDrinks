using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Media;

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
            maxPlayTime = game.maxPlayTime;
            thumbnail = game.thumbnail;
            imageName = game.imageName;
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

        public string thumbnail;
        public string imageName;



    }
}