using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class GamesList : TableLayoutPanel
    {
        public List<Game> games;
        public GamesList(string seach, List<Game> games)
        {
            this.games = games;

            makeItems(seach);
            
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoSize = true;
            //AutoScroll = true;
            VScroll = true;
        }

        public void makeItems(string seach)
        {
            Controls.Clear();

            foreach (var res in games.Where((Game gam) => gam.name.ToLower().Contains(seach) )) {
                GamesItem gameitem = new GamesItem(res);

                Controls.Add(gameitem);
            }
        }
    }
}