using System.Drawing;
using System.Windows.Forms;
using Shared;
using System;
using System.Collections.Specialized;
using Newtonsoft.Json;

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
        private Reservation reservation;

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

        public ReservationItem(ReservationController reservationController, Reservation reservation) {
            this.reservationController = reservationController;
            this.reservation = reservation;
            if (reservation.state == Reservation.State.Denied)
                bgColor = Color.IndianRed;
            else if (reservation.state == Reservation.State.Accepted)
                bgColor = Color.LightSeaGreen;
            else
                bgColor = Color.LightGray;

            name = reservation.name;
            email = reservation.email;
            phone = reservation.phone;
            numPeople = reservation.numPeople;
            time = reservation.time;
            created = reservation.created;
            pending = reservation.state;

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

                reservation.state = Reservation.State.Denied;

                string response = ServerConnection.sendRequest("/submitReservation.aspx",
                    new NameValueCollection() {
                        {"Action", "update"},
                        {"Reservation", JsonConvert.SerializeObject(reservation)}
                    }
                );

                if (response != "updated")
                {
                    Console.WriteLine("failed" + response);
                    return;
                }

                reservationController.updateReservation(reservation);
                update();

                /*foreach (var item in reservationController.calDayList) {
                    item.reservations.Remove(reservation);
                }
                calTab.reservationList.updateCurrentDay(time.Date);
                calTab.pendingReservationList.updateCurrentDay();*/
            };
            Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(reservationController, reservation);
            };

            acceptButton.Click += (s, e) => {
                reservation.state = Reservation.State.Accepted;

                string response = ServerConnection.sendRequest("/submitReservation.aspx",
                    new NameValueCollection() {
                        {"Action", "update"},
                        {"Reservation", JsonConvert.SerializeObject(reservation)}
                    }
                );

                if (response != "updated")
                {
                    Console.WriteLine("failed" + response);
                    return;
                }

                Console.WriteLine(response);

                update(); //TODO: a little hacky
                reservationController.updateReservation(reservation);
                //calTab.reservationList.updateCurrentDay(time.Date);
                //calTab.pendingReservationList.updateCurrentDay();
            };
        }

        public void update() {
            if (reservation.state == Reservation.State.Pending) {
                theRightItems.Controls.Add(twoButtons);
            }
            else if(theRightItems.Controls.Contains(twoButtons))
                theRightItems.Controls.Remove(twoButtons);
        }
    }
}