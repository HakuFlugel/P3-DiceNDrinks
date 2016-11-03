using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GamesTab : TabPage {


        Form form;
        List<Games> games;
        public Action<string> UserSearchText { get; set; }

        public GamesTab(Form form) {
            // For testing purpose only
            games = new List<Games>();
            games.Add(new Games("AHZ2xB", "Secret Hitler", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("J8AkkS", "Dominion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("5ExgGS", "Small Worlds", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("TYE3sj", "Enter The Gundion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));

            Text = "Games";
            this.form = form;
            allControls();

        }

        private void allControls() {
            string seached = null;
            Control[] allControls = {
                gibSeachBar(),
                gibGameBox(seached)
                };

            Controls.AddRange(allControls);
            }
        private Control gibSeachBar() {

            Button but = new Button();
            but.Text = "Pop";
            but.Click += (s, e) => {
                TestPopupbox p = new TestPopupbox(new Product("Køøls Skid","image/location/"));
                p.Show();
            };
            return but;
            NiceTextBox input = new NiceTextBox();
            input.waterMark = "Search...";
            input.clearable = true;
            
            input.Width = form.Width / 3;
            input.Location = new Point(((form.Width - input.Width) / 2), 5);
            
            return input;
        }



        private Control gibSeachBarButton() {
            Button seachButton = new Button();
            
            
            return seachButton;
        }

        private void SeachButton_Click(object sender, EventArgs e) {
            
        }

        private Control gibEditButton() {
            Button editButton = new Button();
            editButton.Size = new Size(134, 48);

            return editButton;
        }

        private Control gibGameBox(string seached) {
            string listOfGames = "";
            Font font = new Font("Microsoft Sans Serif", 12);
            foreach (var item in games)
                listOfGames += item.id + " " + item.name + Environment.NewLine;

            TextBox allGames = new TextBox();
            allGames.Multiline = true;
            allGames.ScrollBars = ScrollBars.Both;
            allGames.Width = form.Width - 40;
            allGames.Height = form.Height - 200;
            allGames.Location = new Point(10, (form.Height - allGames.Height) / 2 - 50);


            allGames.Text = listOfGames;
            allGames.ReadOnly = true;

            return allGames;

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
