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
            RowCount = 2;
            ColumnCount = 2;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);

            Click += (s, e) => {
                //GamePopupBox p = new GamePopupBox(, );
            };

            Controls.Add(new Label { Text = game.name, AutoSize = true, Dock = DockStyle.Left, Font = new Font("Arial",25) });


            Controls.Add(new PictureBox { BackColor = Color.Black, Dock = DockStyle.Right });

            

        }
        
    }
}