using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Collections.Generic;
using System.Net;

namespace AdministratorPanel {
    public class GamePopupBoxRght : TableLayoutPanel {
        
        private TableLayoutPanel gameLisLayoutPanelt = new TableLayoutPanel() {
            Dock = DockStyle.Fill, 
            BorderStyle = BorderStyle.Fixed3D,
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoScroll = true
        };

        private NiceTextBox searchBox = new NiceTextBox() {
            waterMark = "Type to search for a game",
            clearable = true,
            MinimumSize = new Size(384, 0)
        };

        private XmlParser api = new XmlParser();
        private List<Game> games;
        private string searchWord = "";
        private GamePopupBox gamePopupBox;

        public GamePopupBoxRght(GamePopupBox gamePopupBox) {
            this.gamePopupBox = gamePopupBox;
            Controls.Add(searchBox);
            Controls.Add(gameLisLayoutPanelt);

            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Dock = DockStyle.Right;
            AutoSize = true;

            searchBox.KeyPress += (s,e) => {
                if (e.KeyChar != (char)Keys.Enter)
                    return;
                gameSearchApi();
            };
        }
            
        private void gameSearchApi() {
            searchWord = searchBox.Text;
            update();
            searchBox.Text = searchWord;
        }

        private void update() {
            gameLisLayoutPanelt.Controls.Clear();
            try {
                games = api.getGames(searchWord);

            } catch (WebException e) {
                gameLisLayoutPanelt.Controls.Add(new Label() { Name = "Error Connection",
                                                               Text = "Connection to Geekdo.com failed, it could be because of no internet or geekdo is down",
                                                               Size = new Size(384, 100),
                                                               Font = new Font(SystemFonts.DefaultFont.FontFamily, 24),
                                                               Dock = DockStyle.Top});

                NiceMessageBox.Show(e.Message, "Connection Error");
                return;
            } catch (Exception e) {
                NiceMessageBox.Show(e.Message);
            }
            if (games.Count > 0) {
                foreach (var item in games) {
                    gameLisLayoutPanelt.Controls.Add(new XmlGameItem(item, gamePopupBox));
                }
            }
            else if (searchWord != "")
                gameLisLayoutPanelt.Controls.Add(new Label() {
                    Name = "SearchWord Error",
                    Text = "Could not find a game with that name",
                    Size = new Size(384, 100),
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 24),
                    Dock = DockStyle.Top
                });
        }
    }
}