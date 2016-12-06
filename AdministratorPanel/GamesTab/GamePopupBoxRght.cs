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

        private NiceTextBox seachBar = new NiceTextBox() {
            waterMark = "Type to search for a game",
            clearable = true,
            MinimumSize = new Size(384, 0),
        };

        private XmlParser api = new XmlParser();
        private List<Game> games;
        private string searchWord = "";
        private GamePopupBox gamePopupBox;

        public GamePopupBoxRght(GamePopupBox gamePopupBox) {
            this.gamePopupBox = gamePopupBox;
            Controls.Add(seachBar);
            Controls.Add(gameLisLayoutPanelt);

            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Dock = DockStyle.Right;
            AutoSize = true;

            seachBar.KeyPress += (s,e) => {
                if (e.KeyChar != (char)Keys.Enter)
                    return;
                seachApi();
            };
            seachBar.Leave += (s, e) => {
                seachApi();
            };
        }
            
        private void seachApi() {
            searchWord = seachBar.Text;
            update();
            seachBar.Text = searchWord;
        }

        private void update() {
            gameLisLayoutPanelt.Controls.Clear();
            try {
                games = api.getGames(searchWord);

            } catch (WebException e) {
                //TODO: put message in result box instead
                MessageBox.Show(e.Message, "Connection Error");
            } catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            if (games.Count > 0) {
                foreach (var item in games) {
                    gameLisLayoutPanelt.Controls.Add(new XmlGameItem(item, gamePopupBox));
                }
            }
            else if (searchWord != "")
                MessageBox.Show(
                "Could not find anything, there could be two reasons for this:" + Environment.NewLine +
                "1. " + searchWord + " is not a valid BoardGame name " + Environment.NewLine +
                "2. There is no information on " + searchWord + " in the API.",
                "Could not find anything on this" + searchWord);
        }
    }
}