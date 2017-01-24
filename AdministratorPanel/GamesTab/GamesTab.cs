using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Net;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        public List<Game> games;
        public GamesList game;
        private Genres genres = new Genres();
        private string search = "";

        private TableLayoutPanel outerTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill
        };

        private TableLayoutPanel innerTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 2,
            AutoSize = true,
            RowCount = 1,
            Dock = DockStyle.Top
        };

        private Button addGameButton = new Button() {
            Size = new Size(100, 20),
            Dock = DockStyle.Right,
            Text = "Add Game"
        };

        private NiceTextBox seachBar = new NiceTextBox() {
            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0)
        };
        
        public GamesTab(FormProgressBar probar) {
            //name of the tab
            Text = "Games";

            Load();
            download();

            probar.addToProbar();                               //For progress bar. 1

            game = new GamesList(games,this,genres);
            game.makeItems(search);

            probar.addToProbar();                               //For progress bar. 2
                                        
            AutoSize = true;
            Dock = DockStyle.Fill;

            seachBar.KeyPress += (sender, e) => {
                if (e.KeyChar != (char)13) {
                    return;
                }

                search = seachBar.Text.ToLower();

                update();
            };

            addGameButton.Click += (e, s) => {
                new GamePopupBox(this, null, genres);
            };

            outerTableLayoutPanel.Controls.Add(game);
            probar.addToProbar();                               //For progress bar. 3
            Controls.Add(outerTableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 4
            Controls.Add(innerTableLayoutPanel);
            innerTableLayoutPanel.Controls.Add(seachBar);
            probar.addToProbar();                               //For progress bar. 5
            innerTableLayoutPanel.Controls.Add(addGameButton);
            probar.addToProbar();                               //For progress bar. 6
        }

        public override void download()
        {
            try
            {
                string response = ServerConnection.sendRequest("/get.aspx",
                    new NameValueCollection() {
                        {"Type", "Games"}
                    }
                );

                Console.WriteLine(response);
                var nottuple = JsonConvert.DeserializeObject<List<Game>>(response) ?? new List<Game>();

                Console.WriteLine(nottuple);

                foreach (var game in games)
                {
                    if (!nottuple.Any(g => g.imageName == game.imageName))
                    {
                        if (game.imageName == null && File.Exists("images/games/" + game.imageName))
                        {
                            File.Delete("images/games/" + game.imageName);
                        }
                    }
                }

                foreach (var newgame in nottuple)
                {
                    if (!File.Exists("images/games/" + newgame.imageName) || newgame.timestamp > games.FirstOrDefault(g => g.id == newgame.id)?.timestamp)
                    {
                        if (File.Exists("images/games/" + newgame.imageName))
                        {
                            File.Delete("images/games/" + newgame.imageName);
                        }

                        using (WebClient client = new WebClient())
                        {
                            Console.WriteLine("http://" + ServerConnection.ip + "/images/games/" + newgame.imageName);
                            client.DownloadFile(new Uri("http://" + ServerConnection.ip + "/images/games/" + newgame.imageName), "images/games/" + newgame.imageName);
                        }

                    }
                }

                games = nottuple;

                game.makeItems();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void update() {
            seachBar.Text = (seachBar.Text == "" && !seachBar.Focused) ? seachBar.waterMark : search;
            game.makeItems(search);
        }

        public override void Save() {
            genres.Save();
            Directory.CreateDirectory("Sources");
            var json = JsonConvert.SerializeObject(games);
            Directory.CreateDirectory("Sources");

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

            if (games == null) {
                games = new List<Game>();
            }
        }
    }
}
