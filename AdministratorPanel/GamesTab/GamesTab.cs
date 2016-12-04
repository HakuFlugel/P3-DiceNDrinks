using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using Newtonsoft.Json;
using System.IO;

namespace AdministratorPanel {
    public class GamesTab : AdminTabPage {

        public List<Game> games;
        public GamesList game;
        private Genres genres = new Genres();
        private string search = "";
        private FormProgressBar probar;
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
            Text = "Add Game",
        };

        private NiceTextBox seachBar = new NiceTextBox() {
            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0),
        };
        
        public GamesTab(FormProgressBar probar) {
            this.probar = probar;
            Load();
            probar.addToProbar();                               //For progress bar. 1
            

            game = new GamesList(games,this,genres);
            game.makeItems(seach);
            Text = "Games";
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
                GamePopupBox gameBox = new GamePopupBox(this, null, genres);
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

        private void update() {
            seachBar.Text = (seachBar.Text == "") ? seachBar.waterMark : search;
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
