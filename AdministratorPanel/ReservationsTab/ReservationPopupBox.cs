using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using Shared;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using System.Media;

namespace AdministratorPanel {
    class ReservationPopupBox : FancyPopupBox {

        private NiceTextBox reservationName = new NiceTextBox() {
            Width = 200,
            waterMark = "Reservation Name",
            Margin = new Padding(4, 10, 20, 10)
        };

        private NiceTextBox numPeople = new NiceTextBox() {
            Width = 200,
            waterMark = "Number of people",
            Margin = new Padding(4, 0, 20, 10)
        };

        private NiceTextBox phoneNumber = new NiceTextBox() {
            Width = 200,
            waterMark = "Phone number",
            Margin = new Padding(4, 0, 20, 10)
        };

        private NiceTextBox email = new NiceTextBox() {
            Width = 200,
            waterMark = "Email",
            Margin = new Padding(4, 0, 20, 10)
        };

        private DateTimePicker datePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };

        private NiceTextBox timePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        private ComboBox pendingSet = new ComboBox() {
            Text = "Pending",
            DropDownStyle = ComboBoxStyle.DropDownList,
        };

        private TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel() {
            RowCount = 1,
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        private TableLayoutPanel innerRigntTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private TableLayoutPanel innerLeftTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private ReservationController reservationController;
        private Reservation reservation;

        public ReservationPopupBox(ReservationController reservationController, Reservation reservation = null) {
            Text = "Reservation";


            pendingSet.DataSource = Enum.GetValues(typeof(Reservation.State));
            this.reservationController = reservationController;
            this.reservation = reservation;
            if (reservation != null) {
                try {
                    reservationName.Text = reservation.name;
                    numPeople.Text = reservation.numPeople.ToString();
                    if (reservation.phone != null) {
                        phoneNumber.Text = reservation.phone;
                    }
                    if (reservation.email != null) {
                        email.Text = reservation.email;
                    }

                    datePicker.Value = reservation.time.Date;
                    timePicker.Text = reservation.time.ToString("HH:mm");
                    pendingSet.SelectedItem = reservation.state;

                } catch (ArgumentOutOfRangeException) {

                }

            } else {
                pendingSet.SelectedItem = Reservation.State.Accepted;
                Controls.Find("delete", true).First().Enabled = false;
            }

            SubscriptionController();
        }

        private void SubscriptionController() {
            reservationName.TextChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.name != reservationName.Text ? true : false : true; };
            numPeople.TextChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.numPeople.ToString() != numPeople.Text ? true : false : true; };
            phoneNumber.TextChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.phone != phoneNumber.Text ? true : false : true; };
            email.TextChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.email != email.Text ? true : false : true; };
            timePicker.TextChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.time.ToString("HH:mm") != timePicker.Text ? true : false : true; };
            pendingSet.SelectedIndexChanged += (s, e) => { hasBeenChanged = (reservation != null) ? reservation.state != (Reservation.State)Enum.Parse(typeof(Reservation.State), pendingSet.Text) ? true : false : true; };
        }

        protected override Control CreateControls() {

            Controls.Add(headerTableLayoutPanel);

            innerRigntTableLayoutPanel.Controls.Add(reservationName);
            innerRigntTableLayoutPanel.Controls.Add(numPeople);
            innerRigntTableLayoutPanel.Controls.Add(phoneNumber);
            innerRigntTableLayoutPanel.Controls.Add(email);

            innerLeftTableLayoutPanel.Controls.Add(datePicker);
            innerLeftTableLayoutPanel.Controls.Add(timePicker);
            innerLeftTableLayoutPanel.Controls.Add(pendingSet);

            headerTableLayoutPanel.Controls.Add(innerRigntTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(innerLeftTableLayoutPanel);
            return headerTableLayoutPanel;
        }

        protected override void delete(object sender, EventArgs e) {

            if (DialogResult.Yes == NiceMessageBox.Show("Delete Reservation", "Are you sure you want to delete this newReservation?", MessageBoxButtons.YesNo)) {

                //WebClient client = new WebClient();
                //var resp = client.UploadValues("http://172.25.11.113" + "/submitReservation.aspx",
                //    new NameValueCollection() {
                //        {"Action", "delete"},
                //        {"Reservation", JsonConvert.SerializeObject(reservation)}
                //    }
                //);



                string response = ServerConnection.sendRequest("/submitReservation.aspx",
                    new NameValueCollection() {
                        {"Action", "delete"},
                        {"Reservation", JsonConvert.SerializeObject(reservation)}
                    }
                );
                Console.WriteLine(response);

                string[] responsesplit = response.Split(' ');


                if (responsesplit[0] != "deleted")
                {
                    Console.WriteLine("failed to delete reservation");
                    return;
                }

                reservationController.removeReservation(reservation);

            }

            Close();

        }

        protected override void save(object sender, EventArgs e) {

            DateTime expectedDate;
            if (!DateTime.TryParseExact(timePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate)) {

                NiceMessageBox.Show("The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            if (reservationName.Text == reservationName.waterMark || numPeople.Text == reservationName.waterMark) {

                NiceMessageBox.Show("You need to input a name AND a number of people");
                return;
            }
            if (phoneNumber.Text == phoneNumber.waterMark && email.Text == email.waterMark) {

                NiceMessageBox.Show("You need to input a phone number or a email");
                return;
            }
            try {
                emailCheck(email.Text);
            } catch (Exception en) {
                NiceMessageBox.Show(en.Message);
                return;
            }

            //string tempDate = datePicker.Value.ToString("dd-MM-yyyy");
            //string tempTime = timePicker.Text;

            DateTime dt = datePicker.Value.Date.Add(expectedDate.TimeOfDay);

            //DateTime newDate = DateTime.ParseExact(tempDate + " " + tempTime + ":00", "dd-MM-yyyy HH:mm:00",
            //                           CultureInfo.InvariantCulture);

            // actual saving

            Reservation newReservation = new Reservation();
            newReservation.id = reservation?.id ?? 0;
            newReservation.state = (Reservation.State)pendingSet.SelectedValue;
            newReservation.timestamp = DateTime.UtcNow;
            newReservation.name = reservationName.Text;
            int.TryParse(numPeople.Text, out newReservation.numPeople);
            newReservation.phone = phoneNumber.Text;
            newReservation.email = email.Text;
            newReservation.time = dt;
            //cd.fullness += newReservation.numPeople;


            try {
                string response = ServerConnection.sendRequest("/submitReservation.aspx",
                new NameValueCollection() {
                    {"Action", reservation == null ? "add" : "update"},
                    {"Reservation", JsonConvert.SerializeObject(newReservation)}
                }
                
            );
                Console.WriteLine(response);
                if (response.StartsWith("exception")) {
                    throw new Exception(response);
                }

                string[] responsesplit = response.Split(' ');

                if (reservation == null) {

                    if (responsesplit[0] != "added") {
                        Console.WriteLine("wrong response: " + response);
                        return;
                    }

                    int reservationID;
                    if (!int.TryParse(responsesplit[1], out reservationID)) {
                        Console.WriteLine("invalid reservation id returned");
                        return;
                    }

                    newReservation.id = reservationID;
                    reservationController.addReservation(newReservation);

                }
                else {


                    if (responsesplit[0] != "updated") {
                        Console.WriteLine("wrong response: " + response);
                        return;
                    }

                    newReservation.id = reservation.id;
                    reservationController.updateReservation(newReservation);
                }
            }
            catch (Exception) {
                NiceMessageBox.Show("Failed to save to the server, changes will not be send to the server", "Server connection problem");
                return;
            }

            this.Close();
        }

        public void emailCheck(string email) {

            const string validLocalSymbols = "!#$%&'*+-/=?^_`{|}~"; // !#$%&'*+-/=?^_`{|}~      quoted og evt. escaped "(),:;<>@[]
            const string validDomainSymbols = ".-";

            string[] emailParts = email.Split('@');

            if (emailParts.Length != 2)
                throw new FormatException("Email address must contain exactly one '@'");

            if (emailParts[0][0] == '.' || emailParts[0][emailParts.Length - 1] == '.' || emailParts[1][0] == '.' || emailParts[1][emailParts.Length - 1] == '.')
                throw new FormatException("Email address local- or domain-part can't start or end with a '.'");

            if (!emailParts[1].Contains('.'))
                throw new FormatException("Email adress domain part must contain atleast 1 '.'. ie. @domain.tld");

            if (email.Contains(".."))
                throw new FormatException("Email address may not contain consecutive '.'s, ie. '..'.");

            foreach (char ch in emailParts[0]) {
                if (!Char.IsLetterOrDigit(ch) && !validLocalSymbols.Contains(ch))
                    throw new FormatException($"Email address local-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validLocalSymbols }\"");
            }

            foreach (var ch in emailParts[1]) {
                if (!Char.IsLetterOrDigit(ch) && !validDomainSymbols.Contains(ch))
                    throw new FormatException($"Email address domain-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols \"{ validDomainSymbols }\"");
            }
        }
    }
}