using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using Newtonsoft.Json;
using System.IO;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        
        public List<Game> games;
        GamesList game;
        public Action<string> UserSearchText { get; set; }
        string seach ="";

        TableLayoutPanel tb = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
            
        };
        TableLayoutPanel tbtb = new TableLayoutPanel() {
            ColumnCount = 2,
            AutoSize = true,
            RowCount = 1,
            Dock = DockStyle.Top,
            
        };

        Button addGameButton = new Button() {
            Size = new Size(100, 20),
            Dock = DockStyle.Right,
            Text = "Add Game",
        };

        NiceTextBox seachBar = new NiceTextBox() {
            
            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0),

            Margin = new Padding(20, 5, 20, 5),
        };
        
        
        public GamesTab() {

            Load();
            game = new GamesList(seach, games);
            Text = "Games";
            //foreach (var item in games)
            //    Console.WriteLine(item.name);

            //CreateGamesForDebugShit(); // For testing purpose only

            AutoSize = true;
            Dock = DockStyle.Fill;
            BackColor = Color.AliceBlue;

            seachBar.KeyPress += (sender, e) => {

                if (e.KeyChar != (char)13) {
                    return;
                }
                
                seach = seachBar.Text.ToLower();
                
                update();
            };


            addGameButton.Click += (e, s) => {
                GamePopupBox gameBox = new GamePopupBox(this, null);
            };
            
            tb.Controls.Add(game);
            Controls.Add(tb);
            Controls.Add(tbtb);
            tbtb.Controls.Add(seachBar);
            tbtb.Controls.Add(addGameButton);
        }

        private void update() {

            seachBar.Text = seach;
            game.Controls.Clear();
            game.makeItems(seach);
        }


             
        public override void Save() {
            var json = JsonConvert.SerializeObject(games);
            if (!File.Exists(@"Sources/Games.json"))
                File.Create(@"Sources/Games.json");

            using (StreamWriter textWriter = new StreamWriter(@"Sources/Games.json")) {
                foreach (var item in json) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {
            string input;
            if (File.Exists(@"Sources/Games.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/Games.json")) {
                    input = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                if (input != null) {
                    games = JsonConvert.DeserializeObject<List<Game>>(input);
                    
                }
            }
        }





        private void CreateGamesForDebugShit() {
            games = new List<Game>();

            games.Add(new Game {
                bggid = "AHZ2xB",
                name = "Secret Hitler",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "AHO2xB",
                name = "Killer game",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "ABZ2xB",
                name = "Vertical",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "PHZ2xB",
                name = "Shit Storm",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "5ExgGS",
                name = "Small Worlds",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "AHZ2xB",
                name = "Dominion",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "TYE3sj",
                name = "Enter The Gundion",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "TYE3Kj",
                name = "Risk",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
            games.Add(new Game {
                bggid = "TSE3sj",
                name = "Settelers",
                genre = new List<string> { "Horror", "Lying", "Other stuff" },
                description = "A game about gaming",
                publishedYear = 2014,
                minPlayers = 5,
                maxPlayers = 10,
                minPlayTime = 30,
                maxPlayTime = 60,
                imageName = "TosetPictureInFuture"
            });
        }
    }

}
