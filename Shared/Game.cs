using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using System.Text.RegularExpressions;
using System.Media;
using System.Text;

namespace Shared
{
    [Serializable]
    public class Game
    {
        public Game() { }
        public Game(Game game) { //Copy constructor
            if (game != null) {
                bggid = game.bggid;
                name = game.name;
                description = game.description;
                if(game.genre != null)
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
                
        }

        public int id;
        public string bggid;

        public string name;
        public string description;
        public List<string> genre = new List<string>();  
        public int difficulity;             // 0 - 100 hvor 100 er svært og 0 er nemt.
        public int publishedYear;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail;
        public string imageName;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("---------------------------");
            sb.AppendLine("id = " + id.ToString());
            sb.AppendLine("bggid = " + bggid);
            sb.AppendLine("name = " + name);
            sb.AppendLine("publishedYear = " + publishedYear.ToString());
            sb.AppendLine();
            sb.AppendLine("minplayers = " + minPlayers.ToString());
            sb.AppendLine("maxplayers = " + maxPlayers.ToString());
            sb.AppendLine();
            sb.AppendLine("minplaytime = " + minPlayTime.ToString());
            sb.AppendLine("maxplaytime = " + maxPlayTime.ToString());
            sb.AppendLine();
            sb.AppendLine("difficulity = " + difficulity.ToString());
            sb.AppendLine();

            sb.AppendLine("genre");
            if(genre != null)
                foreach (var item in genre) {
                    sb.AppendLine(item);
                }
            sb.AppendLine();
            sb.AppendLine("decription = " + description);
            sb.AppendLine();
            sb.AppendLine("thumbnail = " + thumbnail);
            sb.AppendLine("imagename = " + imageName);
            sb.AppendLine("---------------------------");

            return sb.ToString();
        }

    }
}