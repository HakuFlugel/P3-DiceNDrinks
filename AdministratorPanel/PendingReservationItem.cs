using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using System;

namespace AdministratorPanel {
    public class PendingReservationItem : NiceButton {

        public string name;
        public string email;
        public string phone;
        public int numPeople;
        public DateTime time;
        public bool pending;

        public PendingReservationItem(CalendarDay calDay, List<Reservation> resList) {
            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            /*Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(evntTab, evnt);
                p.Show();
            };*/

            Controls.Add(new Label { Text = "Somefucking date" });
            foreach (var res in resList.FindAll((Reservation reserv) => reserv.pending == true)) {
                Controls.Add(new Label { Text = res.name });
            }
        }
    }
}