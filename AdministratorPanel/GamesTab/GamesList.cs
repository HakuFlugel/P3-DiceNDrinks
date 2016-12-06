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

        private GamesController gamesController;

        public GamesList(GamesController gamesController)
        {
            this.gamesController = gamesController;
            Dock = DockStyle.Fill;

            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;

            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            MinimumSize = new Size(200,440);

            AutoSize = false;
            AutoScroll = true;
            HScroll = false;
        }

        public void makeItems(string seach)
        {
            Controls.Clear();
            if (gamesController.games != null)
                foreach (var res in gamesController.games.Where((Game game) => (game.name != null) ? (game.name.ToLower().Contains(seach)):(game.id.ToString().ToLower().Contains(seach))).OrderBy(o=>  o.name )) {
                    GamesItem gameitem = new GamesItem(res);
                    gameitem.Click += (s, e) => { GamePopupBox popupbox = new GamePopupBox(gamesController,res); };
                    Controls.Add(gameitem);
                }
        }
    }
}