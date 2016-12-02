using System.Drawing;
using System.Windows.Forms;
using Shared;
using System;

namespace AdministratorPanel {
    public class ReservationItem : NiceButton {
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


        public ReservationItem(ReservationController reservationController, Reservation res) {
            this.reservationController = reservationController;
            this.res = res;
            if (res.state == Reservation.State.Denied)
                bgColor = Color.IndianRed;
            else if (res.state == Reservation.State.Accepted)
                bgColor = Color.LightSeaGreen;
            else
                bgColor = Color.LightGray;

            name = res.name;
            email = res.email;
            phone = res.phone;
            numPeople = res.numPeople;
            time = res.time;
            created = res.created;
            pending = res.state;


            RowCount = 1;
            ColumnCount = 3;

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
            acceptButton.Text = "Accept";
            acceptButton.Click += (s, e) => {
                res.pending = false;
                reservationController.updateReservation(res, res); //TODO: a little hacky
                //calTab.reservationList.updateCurrentDay(time.Date);
                //calTab.pendingReservationList.updateCurrentDay();
            };

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
            Console.WriteLine("1");
            foreach (var item in theRightItems.Controls)
                Console.WriteLine(item.ToString());
            Console.WriteLine("2");
            foreach (var item in theMiddleItems.Controls)
                Console.WriteLine(item.ToString());
            Console.WriteLine("3");
            foreach (var item in theLeftItems.Controls)
                Console.WriteLine(item.ToString());
        }

        private void subscribeController() {
            declineButton.Click += (s, e) => {
                //TODO: should we actually have a state for this?? So that the customer can see that it was rejected for some reason...
                //TODO: locking reservations = deny remaining reservations?
                res.state = Reservation.State.Denied;
                reservationController.updateReservation(res, res);
                update();

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
                reservationController.updateReservation(res/*,*/ /*res*7*/);
                //calTab.reservationList.updateCurrentDay(time.Date);
                //calTab.pendingReservationList.updateCurrentDay();
            };

        }



        public void update() {

            if (res.state == Reservation.State.Pending) {
                theRightItems.Controls.Add(twoButtons);
            }
            else if(theRightItems.Controls.Contains(twoButtons))
                theRightItems.Controls.Remove(twoButtons);

        }
    }
}