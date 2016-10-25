using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel {
    class GamesTab : TabPage {
  
        



        public GamesTab() {
            // For testing purpose only
            List<Games> games = new List<Games>();
            games.Add(new Games("AHZ2xB", "Secret Hitler", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("J8AkkS", "Dominion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("5ExgGS", "Small Worlds", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("TYE3sj", "Enter The Gundion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));

            Text = "Games";
         
            
            displayGames(games);

        }
       
       

        private void displayGames(List<Games> games) {

            
            string listOfGames = "";
            Font font = new Font("Microsoft Sans Serif", 12);
            foreach (var item in games)
                listOfGames += item.ToString() + Environment.NewLine;







            TextBox allGames = new TextBox();
            allGames.Multiline = true;
            allGames.ScrollBars = ScrollBars.Both;
            allGames.Dock = DockStyle.Left;

            
            allGames.Text = listOfGames;
            allGames.ReadOnly = true;
            allGames.Location = new Point(5, 10);

            Controls.Add(allGames);





            /*
             In two collums.. Not quite working as I wanted (obviously) but will save this as an reference if need be.
             TextBox[] allGames = {
                new TextBox(), new TextBox() };

            foreach(var item in allGames) {
                item.Multiline = true;
                item.ScrollBars = ScrollBars.Both;
                item.ReadOnly = true;
                item.Location = new Point(5, 10);
            }
            foreach(var item in games) {
                listOfGamesID += item.id + Environment.NewLine;
                listOfGameNames += item.name + Environment.NewLine;
            }
            allGames[1].Text = listOfGameNames; allGames[1].Dock = DockStyle.Left;
            allGames[0].Text = listOfGamesID; allGames[0].Dock = DockStyle.Left;
            
            Controls.AddRange(allGames);
            */

        }
    }
    class Games {
        public string id;

        public string name;
        public string genre;
        public string description;
        public int publishedYear;

        public int minPlayers, maxPlayers;
        public int minPlayTime, maxPlayTime;

        public string thumbnail, image;

        public Games(string id, string name, string genre, string description, int publishedYear, int minPlayers, int maxPlayers, int minPlayTime, int maxPlayTime, string thumbnail) {
            this.id = id;
            this.name = name;
            this.genre = genre;
            this.description = description;
            this.publishedYear = publishedYear;
            this.minPlayers = minPlayers;
            this.maxPlayers = maxPlayers;
            this.minPlayTime = minPlayTime;
            this.maxPlayTime = maxPlayTime;
            this.thumbnail = thumbnail;
        }
    }
}
