using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using Newtonsoft.Json;
using System.IO;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        
        //public List<Game> games;
        public GamesList gamesList;
        string seach ="";
        private FormProgressBar probar;
        public GamesController gamesController;
        

        TableLayoutPanel tb = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill
            
        };
        TableLayoutPanel tbtb = new TableLayoutPanel() {
            ColumnCount = 2,
            AutoSize = true,
            RowCount = 1,
            Dock = DockStyle.Top
            
        };

        Button addGameButton = new Button() {
            Size = new Size(100, 20),
            Dock = DockStyle.Right,
            Text = "Add Game",
        };

        public NiceTextBox seachBar = new NiceTextBox() {
            
            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0),
            //Margin = new Padding(20, 5, 20, 5),
        };

        public GamesTab(GamesController gamesController, FormProgressBar probar)
        {
            this.gamesController = gamesController;
            this.probar = probar;
            Load();
            probar.addToProbar();                               //For progress bar. 1
            if (gamesController.games.Count < 1)
                CreateGamesForDebugShit(); // For testing purpose only

            gamesList = new GamesList(gamesController);
            gamesList.makeItems(seach);
            Text = "Games";
            probar.addToProbar();                               //For progress bar. 2
                                        


            AutoSize = true;
            Dock = DockStyle.Fill;
            //BackColor = Color.AliceBlue;

            seachBar.KeyPress += (sender, e) => {

                if (e.KeyChar != (char)13) {
                    return;
                }
                
                seach = seachBar.Text.ToLower();
                
                update();
            };


            addGameButton.Click += (e, s) => {
                GamePopupBox gameBox = new GamePopupBox(gamesController, null);
            };
            
            tb.Controls.Add(gamesList);
            probar.addToProbar();                               //For progress bar. 3
            Controls.Add(tb);
            probar.addToProbar();                               //For progress bar. 4
            Controls.Add(tbtb);
            tbtb.Controls.Add(seachBar);
            probar.addToProbar();                               //For progress bar. 5
            tbtb.Controls.Add(addGameButton);
            probar.addToProbar();                               //For progress bar. 6
        }

        private void update() {

            seachBar.Text = (seachBar.Text == "") ? seachBar.waterMark : seach;

            gamesList.makeItems(seach);
        }



        public override void Save()
        {
        }

//        {
//            genres.Save();
//            Directory.CreateDirectory("Sources");
//            var json = JsonConvert.SerializeObject(games);
//            Directory.CreateDirectory("Sources");
//
//            using (StreamWriter textWriter = new StreamWriter(@"Sources/Games.json")) {
//                foreach (var item in json) {
//                    textWriter.Write(item.ToString());
//                }
//            }
//        }
//
        public override void Load()
        {
        }

//            string input;
//            if (File.Exists(@"Sources/Games.json")) {
//                using (StreamReader streamReader = new StreamReader(@"Sources/Games.json")) {
//                    input = streamReader.ReadToEnd();
//                    streamReader.Close();
//                }
//                if (input != null) {
//                    games = JsonConvert.DeserializeObject<List<Game>>(input);
//
//                }
//            }
//
//            //TODO: do this for all tabs?
//            if (games == null)
//            {
//                games = new List<Game>();
//            }
//        }





        private void CreateGamesForDebugShit()
        {
            var games = gamesController.games;

            games.Add(new Game {
                id = 0,
                bggid = "AHZ2xB",
                name = "Secret Hitler",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                difficulity = 10,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 1,
                bggid = "AHO2xB",
                name = "Killer gamesList",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 7,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 2,
                bggid = "ABZ2xB",
                name = "Vertical",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 10,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 3,
                bggid = "PHZ2xB",
                name = "Shit Storm",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 3,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 4,
                bggid = "5ExgGS",
                name = "Small Worlds",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 5,
                bggid = "AHZ2xB",
                name = "Dominion",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 6,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 6,
                bggid = "TYE3sj",
                name = "Enter The Gundion",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 7,
                bggid = "TYE3Kj",
                name = "Risk",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 4,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                id = 8,
                bggid = "TSE3sj",
                name = "Settelers",
                description = "A gamesList about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                difficulity = 2,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
        }
    }

}
