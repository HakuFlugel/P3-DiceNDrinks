using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;
using System.Collections.Generic;
using System.Net;

namespace AdministratorPanel {
    public class GamePopupBoxRght : TableLayoutPanel {
        
        TableLayoutPanel gameList = new TableLayoutPanel() {
            Dock = DockStyle.Fill, //TODO: FAFSAJFASDQWHFBQKFHQWIFQWHFAWISFHAIFHWQIF

            BorderStyle = BorderStyle.Fixed3D,
            ColumnCount = 1,

            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            //MinimumSize = new Size(200, 440),

            //AutoSize = true,
            AutoScroll = true
            
        };

        NiceTextBox seachBar = new NiceTextBox() {

            waterMark = "Type something to seach..",
            clearable = true,
            MinimumSize = new Size(200, 0),

            //Margin = new Padding(5, 5, 20, 5),
        };

        private XmlParser api = new XmlParser();
        List<Game> games;
        string seach = "";
        private GamePopupBox gamePopupBox;

        public GamePopupBoxRght(GamePopupBox gamePopupBox) {
            this.gamePopupBox = gamePopupBox;
            Controls.Add(seachBar);
            Controls.Add(gameList);

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
            seach = seachBar.Text;
            update();
            seachBar.Text = seach;
        }
    

        private void update() {
            gameList.Controls.Clear();
            try {
                using (var client = new WebClient()) {
                    using (var stream = client.OpenRead("http://www.google.com")) { }
                }
                games = api.getGames(seach);

            } catch (WebException) {
                MessageBox.Show("Do hast nich interneten, please connect to an internet source to use this AMAZEBALLZ thingy!", "No internetious");
            } catch (Exception e) {
                MessageBox.Show(e.Message.ToString());
            }
            if (games.Count > 0)
                foreach (var item in games) {
                    gameList.Controls.Add(new XmlGameItem(item, gamePopupBox));
                    
                } else if (seach != "")
                MessageBox.Show(
                    "Could not find anything, there could be two reasons for this:" + Environment.NewLine +
                    "1. " + seach + " is not a valid BoardGame name " + Environment.NewLine +
                    "2. There is no information on " + seach + " in the API.",
                    "Could not find anything on this" + seach);


        }
    }
}