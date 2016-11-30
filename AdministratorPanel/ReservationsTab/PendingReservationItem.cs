using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Shared;
using System;

namespace AdministratorPanel {
    public class PendingReservationItem : NiceButton {
        // TODO: maybe update on event???
        public PendingReservationItem(Calendar cal, ReservationTab calTab, DateTime calDay, List<Reservation> resList) {
            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                cal.SelectionStart = calDay.Date;
                calTab.reservationList.makeItems(calDay.Date);
            };

            Controls.Add(new Label { Text = calDay.Date.ToString("dd. MMMMM yyyy"), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Font = new Font("Arial", 12) });
            foreach (var res in resList.FindAll((Reservation reserv) => reserv.state == Reservation.State.Accepted)) {
                Controls.Add(new Label { Text = res.name + " | " + res.numPeople, Dock = DockStyle.Fill });
            }
        }
    }
}