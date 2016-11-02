using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class ReservationList : TableLayoutPanel
    {
        private Calendar calendar;
        private List<Reservation> reservations;

        public ReservationList(Calendar calendar, List<Reservation> reservations)
        {
            this.calendar = calendar;
            this.reservations = reservations;

            calendar.DateSelected += (sender, args) => { this.makeItems(args.Start);};
            makeItems(DateTime.Today);

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoScroll = true;
            VScroll = true;
        }

        public void makeItems(DateTime day)
        {
            Controls.Clear();

            foreach (var res in reservations.Where((Reservation res) => res.time.Date == day.Date))
            {
                ReservationItem reservationItem = new ReservationItem(res);

                Controls.Add(reservationItem);
            }
        }
    }
}