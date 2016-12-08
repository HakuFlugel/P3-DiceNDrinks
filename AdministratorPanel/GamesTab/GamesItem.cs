using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class GamesItem : NiceButton {

        private TableLayoutPanel gameInformationLeft = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2
        };

        private TableLayoutPanel gameInformationMiddle = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2
        };

        private TableLayoutPanel gameInformationRight = new TableLayoutPanel() {
            ColumnCount = 1,
            RowCount = 2
        };

        private Game game;
        public GamesItem(Game game) {
            RowCount = 1;
            this.game = game;
            ColumnCount = 4;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            
            gameInformationLeft.Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            gameInformationLeft.Controls.Add(new Label { Text = game.bggid, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            Controls.Add(gameInformationLeft);
            
            gameInformationMiddle.Controls.Add(new Label { Text = "min players: " + game.minPlayers, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            gameInformationMiddle.Controls.Add(new Label { Text = "max players: " + game.maxPlayers, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            Controls.Add(gameInformationMiddle);
            
            gameInformationRight.Controls.Add(new Label { Text = "min time: " + game.minPlayTime, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            gameInformationRight.Controls.Add(new Label { Text = "max time: " + game.maxPlayTime, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            
            Controls.Add(gameInformationRight);


            try {
                Controls.Add(new Panel() { Dock = DockStyle.Right, Size = new Size(128, 128), BackgroundImage = Image.FromFile($"images/{game.imageName}"), BackgroundImageLayout = ImageLayout.Zoom, BackColor = Color.Gray, });
            } catch (Exception) {

            }
        }
    }
}