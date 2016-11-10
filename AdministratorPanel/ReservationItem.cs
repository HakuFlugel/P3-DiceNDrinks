using System.Drawing;
using System.Windows.Forms;
using Shared;
using System;

namespace AdministratorPanel
{
    public class ReservationItem : NiceButton
    {
        public string name;
        public string email;
        public string phone;
        public int numPeople;
        public DateTime time;
        public bool pending;

        public ReservationItem(CalendarTab calTab, Reservation res)
        {
            name = res.name;
            email = res.email;
            phone = res.phone;
            numPeople = res.numPeople;
            time = res.time;
            pending = res.pending;

            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(calTab, res);
                p.Show();
            };

            Controls.Add(new Label{ Text = name, AutoSize = true}); // TODO: add content from reservation
        }

    }
}