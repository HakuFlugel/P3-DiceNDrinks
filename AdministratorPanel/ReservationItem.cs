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
            ColumnCount = 3;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(calTab, res);
                p.Show();
            };
            TableLayoutPanel theRightItems = new TableLayoutPanel();
            theRightItems.AutoSize = true;

            TableLayoutPanel theMiddleItems = new TableLayoutPanel();
            theMiddleItems.AutoSize = true;
            theMiddleItems.Dock = DockStyle.Top;

            TableLayoutPanel theLeftItems = new TableLayoutPanel();
            theLeftItems.AutoSize = true;
            theLeftItems.Dock = DockStyle.Right;

            FlowLayoutPanel twoButtons = new FlowLayoutPanel();
            twoButtons.AutoSize = true;
            Button acceptButton = new Button();
            twoButtons.Controls.Add(acceptButton);
            acceptButton.Text = "Accept";
            acceptButton.Click += (s, e) => {
                res.pending = false;
                Console.WriteLine("fish");
                calTab.reserveationList.makeItems(time.Date);
                calTab.pendingReservationList.makeItems();
            };


            Button declineButton = new Button();
            twoButtons.Controls.Add(declineButton);
            declineButton.Text = "Decline";
            declineButton.Click += (s, e) => {
                foreach (var item in calTab.calDayList) {
                    item.reservations.Remove(res);
                }
                calTab.reserveationList.makeItems(time.Date);
                calTab.pendingReservationList.makeItems();
            };

            Controls.Add(theRightItems);
            Controls.Add(theMiddleItems);
            Controls.Add(theLeftItems);


            theRightItems.Controls.Add(new Label { Text = name, AutoSize = true }); // TODO: add content from reservation
            theMiddleItems.Controls.Add(new Label { Text = numPeople.ToString() + " People", AutoSize = true });
            theLeftItems.Controls.Add(new Label { Text = time.ToString("ddddd, dd. MMMM, yyyy HH:mm"), AutoSize = true, Dock = DockStyle.Right });
            theRightItems.Controls.Add(new Label { Text = email, AutoSize = true });
            theMiddleItems.Controls.Add(new Label { Text = phone, AutoSize = true });
            if (res.pending == true) {
                theLeftItems.Controls.Add(twoButtons);
            }

        }
    }
}