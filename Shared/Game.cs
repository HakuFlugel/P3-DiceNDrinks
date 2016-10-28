using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class Game
    {
        public string id;

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