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
        private Calendar calendar;
        private List<Reservation> reservations;

        public GamesList(Calendar calendar, List<Reservation> reservations)
        {
            string seach = "";
            this.calendar = calendar;
            this.reservations = reservations;
            
            makeItems(seach);

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoScroll = true;
            VScroll = true;
        }

        public void makeItems(string seach)
        {
            Controls.Clear();

            foreach (var res in reservations.Where((Reservation res) => res.time.Date == day.Date))
            {
                GamesItem reservationItem = new GamesItem(res);

                Controls.Add(reservationItem);
            }
        }
    }
}