using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministratorPanel {
    class GamesTab : TabPage {
        private Form form;
        private List<Games> games;
        



        public GamesTab(Form form, List<Games> games) {
            Text = "Games";
            this.form = form;
            this.games = games;
            
            displayGames();

        }
       
       

        private void displayGames() {

            int formWidth = form.Width;
            int formHeight = form.Height;
            string listOfGames = "";
            Font font = new Font("Microsoft Sans Serif", 12);
            foreach (var item in games)
                listOfGames += item.ToString() + Environment.NewLine;

            TextBox allGames = new TextBox();
            allGames.Multiline = true;
            allGames.ScrollBars = ScrollBars.Both;
            allGames.Width = formWidth - 35;
            allGames.Height = formHeight - 150;
            allGames.Font = font;
            allGames.Text = listOfGames;
            allGames.ReadOnly = true;
            allGames.Location = new Point(5, 10);

            Controls.Add(allGames);

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
        public override string ToString() {

            return id + " " + name;
        }
    }
}
