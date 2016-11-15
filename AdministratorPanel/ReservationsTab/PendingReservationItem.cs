using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using System;

namespace AdministratorPanel {
    public class PendingReservationItem : NiceButton {

        public PendingReservationItem(Calendar cal, CalendarTab calTab, DateTime calDay, List<Reservation> resList) {
            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                cal.SelectionStart = calDay.Date;
                calTab.reserveationList.makeItems(calDay.Date);
            };

            Controls.Add(new Label { Text = calDay.Date.ToString("dd. MMMMM yyyy"), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Font = new Font("Arial", 12) });
            foreach (var res in resList.FindAll((Reservation reserv) => reserv.pending == true)) {
                Controls.Add(new Label { Text = res.name + " | " + res.numPeople, Dock = DockStyle.Fill });
            }
        }
    }
}