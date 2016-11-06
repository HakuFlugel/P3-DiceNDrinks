using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        
        List<Game> games;
        public Action<string> UserSearchText { get; set; }
        string seach ="";
        

        public GamesTab() {

            Text = "Games";
            CreateGamesForDebugShit();// For testing purpose only

            update();
        }

        private void update() {
            Controls.Clear();
            
            AutoSize = true;
            Dock = DockStyle.Fill;
            BackColor = Color.AliceBlue;
            Panel top = new Panel();
            top.Dock = DockStyle.Top;

            Controls.Add(allControls());
        }

        private TableLayoutPanel allControls() {
            TableLayoutPanel tb = new TableLayoutPanel();
            tb.ColumnCount = 1;
            tb.RowCount = 3;
            
            tb.Dock = DockStyle.Fill;
            Panel seachPanel = new Panel();
            seachPanel.AutoSize = true;
            seachPanel.Dock = DockStyle.Top;
            seachPanel.Controls.Add(gibSeachBar());
            tb.Controls.Add(seachPanel);
            GamesList game = new GamesList(seach, games);
            tb.Controls.Add(game);




            return tb;
        }



        private Control gibSeachBar() {
            NiceTextBox seachBar = new NiceTextBox();
            seachBar.Text = seach;
            seachBar.waterMark = "Type something to seach..";
            seachBar.clearable = true;
            
            seachBar.Margin = new Padding(20, 5, 20, 5);
            seachBar.KeyPress += (sender, e) => {
                
                if (e.KeyChar != (char)13) {
                    return;
                }
                seach = seachBar.Text.ToLower();
                update();
            };
            return seachBar;
        }


             
        public override void Save() {
            //throw new NotImplementedException();
        }

        public override void Load() {
            //throw new NotImplementedException();
        }





        private void CreateGamesForDebugShit() {
            games = new List<Game>();

            games.Add(new Game {
                id = "AHZ2xB",
                name = "Secret Hitler",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "AHO2xB",
                name = "Killer game",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "ABZ2xB",
                name = "Vertical",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "PHZ2xB",
                name = "Shit Storm",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "5ExgGS",
                name = "Small Worlds",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "AHZ2xB",
                name = "Dominion",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "TYE3sj",
                name = "Enter The Gundion",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "TYE3Kj",
                name = "Risk",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = "TSE3sj",
                name = "Settelers",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                image = "TosetPictureInFuture"
            });
        }
    }

}
