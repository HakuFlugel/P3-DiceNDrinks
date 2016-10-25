using System;
using System.Xml;
using System.Xml.Serialization;

namespace Shared
{
    [Serializable]
    public class Game
    {
        public string id;

        public string name;
        public string description;
        public int publishedYear;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail, image;
    }
}