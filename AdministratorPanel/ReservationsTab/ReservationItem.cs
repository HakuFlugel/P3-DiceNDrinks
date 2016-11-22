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
        public DateTime created;
        public bool pending;

        public ReservationItem(ReservationController reservationController, Reservation res)
        {
            name = res.name;
            email = res.email;
            phone = res.phone;
            numPeople = res.numPeople;
            time = res.time;
            created = res.created;
            pending = res.pending;


            RowCount = 1;
            ColumnCount = 3;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(reservationController, res);
                p.Show();
            };

            TableLayoutPanel theLeftItems = new TableLayoutPanel();
            theLeftItems.AutoSize = true;

            TableLayoutPanel theMiddleItems = new TableLayoutPanel();
            theMiddleItems.AutoSize = true;

            TableLayoutPanel theRightItems = new TableLayoutPanel();
            theRightItems.AutoSize = true;
            theRightItems.Dock = DockStyle.Right;

            FlowLayoutPanel twoButtons = new FlowLayoutPanel();
            twoButtons.AutoSize = true;
            twoButtons.Dock = DockStyle.Right;

            Button acceptButton = new Button();
            twoButtons.Controls.Add(acceptButton);
            acceptButton.Text = "Accept";
            acceptButton.Click += (s, e) => {
                res.pending = false;
                reservationController.updateReservation(res, res); //TODO: a little hacky
                //calTab.reservationList.makeItems(time.Date);
                //calTab.pendingReservationList.makeItems();
            };

            Button declineButton = new Button();
            twoButtons.Controls.Add(declineButton);
            declineButton.Text = "Decline";
            declineButton.Click += (s, e) => {
                //TODO: should we actually have a state for this?? So that the customer can see that it was rejected for some reason...
                MessageBox.Show("This feature is probably not fully implemented...");
                reservationController.removeReservation(res);
                /*foreach (var item in reservationController.calDayList) {
                    item.reservations.Remove(res);
                }
                calTab.reservationList.makeItems(time.Date);
                calTab.pendingReservationList.makeItems();*/
            };

            Controls.Add(theLeftItems);
            Controls.Add(theMiddleItems);
            Controls.Add(theRightItems);
            //           theWrongItems


            theLeftItems.Controls.Add(new Label { Text = name, AutoSize = true }); // TODO: add content from reservation
            theMiddleItems.Controls.Add(new Label { Text = numPeople.ToString() + " People", AutoSize = true });
            theRightItems.Controls.Add(new Label { Text = "Created: " + created.ToString("ddddd, dd. MMMM, yyyy HH:mm"), AutoSize = true, Dock = DockStyle.Right });
            theRightItems.Controls.Add(new Label { Text = time.ToString("ddddd, dd. MMMM, yyyy HH:mm"), AutoSize = true, Dock = DockStyle.Right });
            theLeftItems.Controls.Add(new Label { Text = email, Width = 150 });
            theMiddleItems.Controls.Add(new Label { Text = phone, AutoSize = true });
            if (res.pending == true) {
                theRightItems.Controls.Add(twoButtons);
            }

        }
    }
}