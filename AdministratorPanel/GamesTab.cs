using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        
        List<Games> games;
        public Action<string> UserSearchText { get; set; }

        

        public GamesTab() {
            
            Text = "Games";
            AutoSize = true;
            Dock = DockStyle.Fill;
            
            Panel top = new Panel();
            top.Dock = DockStyle.Top;
            

            // For testing purpose only
            games = new List<Games>();
            games.Add(new Games("AHZ2xB", "Secret Hitler", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("J8AkkS", "Dominion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("5ExgGS", "Small Worlds", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));
            games.Add(new Games("TYE3sj", "Enter The Gundion", "Horror", "A game about gaming", 2014, 5, 10, 30, 60, "TosetPictureInFuture"));



            Controls.Add(allControls());
        }


        private TableLayoutPanel allControls() {
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.ColumnCount = 1;
            tb.RowCount = 3;
            tb.Dock = DockStyle.Top;
            Panel seachPanel = new Panel();
            seachPanel.AutoSize = true;
            seachPanel.Dock = DockStyle.Top;
            seachPanel.Controls.Add(gibSeachBar());
            tb.Controls.Add(seachPanel);

            tb.Controls.Add(gibGameBox());




            return tb;
        }



        private Control gibSeachBar() {
            NiceTextBox seachBar = new NiceTextBox();
            seachBar.waterMark = "Type something to seach..";
            seachBar.clearable = true;
            seachBar.Margin = new Padding(20, 5, 20, 5);
            seachBar.KeyPress += (sender, e) => { 

                if (e.KeyChar != (char)13) {
                    return;
                }
                Console.WriteLine("Not implimented yet.");
            };
            return seachBar;
        }

        

        private Control gibEditButton() {
            Button editButton = new Button();
            editButton.Size = new Size(134, 48);

            return editButton;
        }

        private TableLayoutPanel gibGameBox() {
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.ColumnCount = 1;
            tb.RowCount = 0;
            tb.AutoSize = true;
            tb.AutoScroll = true;

            foreach (var item in games) {
                tb.RowCount += 1;
                tb.Controls.Add(gibGames(item));
            }

            

            return tb;

            
        }

        private Control gibGames(Games item) {
            hej.exe();



            return game;
        }

        private void gameCLICK(Games game) {
            throw new NotImplementedException();
        }
        
        public override void Save() {
            //throw new NotImplementedException();
        }

        public override void Load() {
            //throw new NotImplementedException();
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
