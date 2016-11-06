using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class CalendarTab : AdminTabPage {
        private Calendar calendar;
        private List<Room> rooms;
        public List<CalendarDay> reservations = new List<CalendarDay>();

        public CalendarTab() {

            for (int i = 0; i < 10; i++)
            {
                Reservation res = new Reservation();
                res.time = DateTime.Now;

                //reservations.Add(res); TODO: fix
            }

            Text = "Calendar";

            TableLayoutPanel outerTable = new TableLayoutPanel();
            outerTable.Dock = DockStyle.Fill;
            outerTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            outerTable.RowCount = 1;
            outerTable.ColumnCount = 2;
            Controls.Add(outerTable);

            //// Left Side
            TableLayoutPanel leftTable = new TableLayoutPanel();
            leftTable.Dock = DockStyle.Left;
            leftTable.AutoSize = true;
            leftTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //leftTable.BorderStyle = BorderStyle.Fixed3D;
            outerTable.Controls.Add(leftTable);

            calendar = new Calendar();
            //calendar.Dock = DockStyle.Top;
            //calendar.Anchor = AnchorStyles.Top;
            leftTable.Controls.Add(calendar);

            PendingReservationList b = new PendingReservationList(this);
            leftTable.Controls.Add(b);

            //// Right side
            //ReservationList rightTable = new ReservationList(calendar, reservations); TODO: fix
            //rightTable.makeItems(reservations, DateTime.Today);
            //outerTable.Controls.Add(rightTable); TODO: fix




            /*

*/
        }

        public override void Save() {
            //throw new NotImplementedException();
        }

        public override void Load() {
            //throw new NotImplementedException();
        }
    }
}
