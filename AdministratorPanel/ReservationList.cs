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
        private CalendarTab calTab;

        public ReservationList(Calendar calendar, CalendarTab calTab)
        {
            this.calendar = calendar;
            this.calTab = calTab;

            calendar.DateSelected += (sender, args) => { this.makeItems(args.Start);};
            makeItems(calendar.SelectionStart);

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
            
            CalendarDay cd = calTab.calDayList.Find(o => o.theDay.Date == day.Date);
            if (cd == null) {
                return;
            }
            calendar.SelectionStart = cd.theDay.Date;

            foreach (var res in cd.reservations) {
                ReservationItem reservationItem = new ReservationItem(calTab, res);

                Controls.Add(reservationItem);
            }
        }
    }
}