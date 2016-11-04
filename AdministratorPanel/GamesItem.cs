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
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            

            Controls.Add(new Label{ Text = game.id, AutoSize = true}); // TODO: add content from reservation
            Controls.Add(new Label { Text = game.name, AutoSize = true });

        }
        protected override void OnClick(EventArgs e) {


            base.OnClick(e);
        }
    }
}