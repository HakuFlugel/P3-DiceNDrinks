using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class CalendarTab : AdminTabPage {
        private Calendar calendar;
        private List<Room> rooms;
        public List<CalendarDay> CalDay = new List<CalendarDay>();
        

        public CalendarTab() {
            CalendarDay test = new CalendarDay() { fullness = 1, isFullChecked = false, reservations = new List<Reservation>() };
            for (int i = 0; i < 3; i++)
            {
                test.reservations.Add(new Reservation { time = DateTime.Now, name = "fish" + i, pending = true });
            }
            for (int j = 0; j < 10; j++) 
            {    
                CalDay.Add(test);
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
