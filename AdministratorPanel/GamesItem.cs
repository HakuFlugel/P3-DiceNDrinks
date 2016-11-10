using System;
using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class GamesItem : NiceButton
    {

        public GamesItem(Game game)
        {
            RowCount = 1;
            ColumnCount = 4;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);

           

            TableLayoutPanel x1 = new TableLayoutPanel();
            x1.ColumnCount = 1;
            x1.RowCount = 2;
            x1.Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            x1.Controls.Add(new Label { Text = game.bggid, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            Controls.Add(x1);

            TableLayoutPanel x2 = new TableLayoutPanel();
            x2.ColumnCount = 1;
            x2.RowCount = 2;
            x2.Controls.Add(new Label { Text = "min players: " + game.minPlayers, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            x2.Controls.Add(new Label { Text = "max players: " + game.maxPlayers, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            Controls.Add(x2);

            TableLayoutPanel x3 = new TableLayoutPanel();
            x3.ColumnCount = 1;
            x3.RowCount = 2;
            x3.Controls.Add(new Label { Text = "min time: " + game.minPlayTime, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });
            x3.Controls.Add(new Label { Text = "max time: " + game.maxPlayTime, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial", 15) });

            
            Controls.Add(x3);

            Controls.Add(new PictureBox { BackColor = Color.Black, Dock = DockStyle.Right });
            
            

        }

        protected override void OnDoubleClick(EventArgs e) {
            base.OnDoubleClick(e);

            GamePopupBox p = new GamePopupBox();

        }

    }
}