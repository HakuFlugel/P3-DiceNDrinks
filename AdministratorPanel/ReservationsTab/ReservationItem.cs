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
        public Reservation.State pending;
        private ReservationController reservationController;
        private Reservation res;


        Button declineButton = new Button() {
            Text = "Decline",
        };
        Button acceptButton = new Button() {
            Text = "Accept",

        };

        TableLayoutPanel theLeftItems = new TableLayoutPanel() {
            AutoSize = true,
        };

        TableLayoutPanel theRightItems = new TableLayoutPanel() {
            AutoSize = true,
            Dock = DockStyle.Right,
        };

        FlowLayoutPanel twoButtons = new FlowLayoutPanel() {
            AutoSize = true,
            Dock = DockStyle.Right,
        };

        TableLayoutPanel theMiddleItems = new TableLayoutPanel() {
            AutoSize = true,
        };

        
        public ReservationItem(ReservationController reservationController, Reservation res)
        {
            this.reservationController = reservationController;
            this.res = res;


            name = res.name;
            email = res.email;
            phone = res.phone;
            numPeople = res.numPeople;
            time = res.time;
            created = res.created;
            pending = res.state;


            RowCount = 1;
            ColumnCount = 3;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            
            controlsController();
            subscribeController();
            update();
        }

        private void controlsController() {

            twoButtons.Controls.Add(acceptButton);

            twoButtons.Controls.Add(declineButton);

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
        }

        private void subscribeController() {
            declineButton.Click += (s, e) => {
                //TODO: should we actually have a state for this?? So that the customer can see that it was rejected for some reason...
                //TODO: locking reservations = deny remaining reservations?
                MessageBox.Show("This feature is probably not fully implemented...");
                reservationController.removeReservation(res);
                /*foreach (var item in reservationController.calDayList) {
                    item.reservations.Remove(res);
                }
                calTab.reservationList.updateCurrentDay(time.Date);
                calTab.pendingReservationList.updateCurrentDay();*/
            };
            Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(reservationController, res);
                p.Show();
            };

            acceptButton.Click += (s, e) => {
                res.state = Reservation.State.Accepted;
                update(); //TODO: a little hacky
                //calTab.reservationList.updateCurrentDay(time.Date);
                //calTab.pendingReservationList.updateCurrentDay();
            };

        }


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