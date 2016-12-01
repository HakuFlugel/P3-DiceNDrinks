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
                addedDate = game.addedDate;
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
        public DateTime addedDate;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail;
        public string imageName;

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("---------------------------");
            sb.AppendLine("id = " + id);
            sb.AppendLine("bggid = " + bggid);
            sb.AppendLine("name = " + name);
            sb.AppendLine("publishedYear = " + publishedYear);
            sb.AppendLine("added = " + addedDate);
            sb.AppendLine();
            sb.AppendLine("minplayers = " + minPlayers);
            sb.AppendLine("maxplayers = " + maxPlayers);
            sb.AppendLine();
            sb.AppendLine("minplaytime = " + minPlayTime);
            sb.AppendLine("maxplaytime = " + maxPlayTime);
            sb.AppendLine();
            sb.AppendLine("difficulity = " + difficulity);
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