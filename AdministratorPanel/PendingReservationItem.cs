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

        public PendingReservationItem(List<Reservation> reslist) {
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
            foreach (var res in reslist.FindAll((Reservation reserv) => reserv.pending == true)) {
                Controls.Add(new Label { Text = res.name });
            }
            
            Controls.Add(new Label{ Text = "Dinmor"});
        }
    }
}