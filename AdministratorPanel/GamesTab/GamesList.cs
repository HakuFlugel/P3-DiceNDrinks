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
        public GamesTab gametab;
        public GamesList( List<Game> games, GamesTab gametab)
        {
            this.games = games;
            this.gametab = gametab;
            Dock = DockStyle.Fill;

            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;

            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            MinimumSize = new Size(200,440);

            AutoSize = false;
            AutoScroll = true;
            //VScroll = true;
            HScroll = false;
        }

        public void makeItems(string seach)
        {
            Controls.Clear();
            if (games != null)
                foreach (var res in games.Where((Game gam) => (gam.name.ToLower().Contains(seach)))) {
                    GamesItem gameitem = new GamesItem(res);
                    gameitem.Click += (s, e) => { GamePopupBox popupbox = new GamePopupBox(gametab,res); };
                    Controls.Add(gameitem);
                }
        }
    }
}