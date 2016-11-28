using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;

namespace AdministratorPanel {
    class ReservationPopupBox : FancyPopupBox {
        NiceTextBox reservationName = new NiceTextBox() {
            Width = 200,
            waterMark = "Reservation Name",
            Margin = new Padding(4, 10, 20, 10)
        };
        NiceTextBox numPeople = new NiceTextBox() {
            Width = 200,
            waterMark = "Number of people",
            Margin = new Padding(4, 0, 20, 10)
        };
        NiceTextBox phoneNumber = new NiceTextBox() {
            Width = 200,
            waterMark = "Phone number",
            Margin = new Padding(4, 0, 20, 10)
        };
        NiceTextBox email = new NiceTextBox() {
            Width = 200,
            waterMark = "Email",
            Margin = new Padding(4, 0, 20, 10)
        };

        DateTimePicker datePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };
        NiceTextBox timePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };
        CheckBox pendingSet = new CheckBox() {
            Checked = false,
            Text = "Pending"
        };


        private ReservationController reservationController;
        private Reservation reservation;

        public ReservationPopupBox(ReservationController reservationController, Reservation reservation = null) {
            this.reservationController = reservationController;
            this.reservation = reservation;
            if (reservation != null) {
                try {
                    reservationName.Text = reservation.name;
                    numPeople.Text = reservation.numPeople.ToString();
                    /*TODO: better way than this?:*/
                    if (reservation.phone != null) {
                        phoneNumber.Text = reservation.phone;
                    }
                    if (reservation.email != null) {
                        email.Text = reservation.email;
                    }
                    datePicker.Value = reservation.time.Date;
                    timePicker.Text = reservation.time.ToString("HH:mm");
                    pendingSet.Checked = reservation.pending;
                }
                catch (ArgumentOutOfRangeException) {

                }

            }
            else {
                Controls.Find("delete", true).First().Enabled = false;
            }
        }



        protected override Control CreateControls() {

            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            TableLayoutPanel rght = new TableLayoutPanel();
            rght.ColumnCount = 1;
            rght.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rght.Dock = DockStyle.Fill;

            rght.Controls.Add(reservationName);
            rght.Controls.Add(numPeople);
            rght.Controls.Add(phoneNumber);
            rght.Controls.Add(email);

            TableLayoutPanel lft = new TableLayoutPanel();
            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            lft.Dock = DockStyle.Fill;

            lft.Controls.Add(datePicker);
            lft.Controls.Add(timePicker);
            lft.Controls.Add(pendingSet);


            header.Controls.Add(rght);
            header.Controls.Add(lft);
            return header;
        }

        protected override void delete(object sender, EventArgs e) {
            if (DialogResult.Yes == MessageBox.Show("Delete Reservation", "Are you sure you want to delete this newReservation?", MessageBoxButtons.YesNo)) {

                reservationController.removeReservation(reservation);

                //TODO: move to event on list... _.. er auto-rename
                //_reservationController.reserveationList.makeItems(DateTime.Today.Date);
                //_reservationController.pendingReservationList.makeItems();
            }
        }

        protected override void save(object sender, EventArgs e) {

            //TODO: kan vi ikke bruge Validate til det her?

            DateTime expectedDate;
            if (!DateTime.TryParseExact(timePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate)) {

                MessageBox.Show("The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            if ((reservationName.Text == reservationName.waterMark || numPeople.Text == reservationName.waterMark)) {
                MessageBox.Show("You need to input a name AND a number of people");
                return;
            }
            if (phoneNumber.Text == phoneNumber.waterMark && email.Text == email.waterMark) {
                MessageBox.Show("You need to input a phone number or a email");
                return;
            }
            try {
                emailCheck(email.Text);
            }
            catch (Exception en) {
                MessageBox.Show(en.Message);
                return;
            }

            string tempDate = datePicker.Value.ToString("dd-MM-yyyy");
            string tempTime = timePicker.Text;

            /*COPY PASTE(SOME OF IT!!)*/
            DateTime newDate = DateTime.ParseExact(tempDate + " " + tempTime + ":00", "dd-MM-yyyy HH:mm:00",
                                       CultureInfo.InvariantCulture);
            /*END OF COPY PASTE*/


            ////////////// actual saving

            Reservation newres = new Reservation();

            newres.pending = pendingSet.Checked;
            newres.name = reservationName.Text;
            int.TryParse(numPeople.Text, out newres.numPeople); // TODO: not handling invalid value here
            newres.phone = phoneNumber.Text;
            newres.email = email.Text;
            newres.time = newDate;
            //cd.fullness += newReservation.numPeople;

            if (reservation == null)
            {
                reservationController.addReservation(newres);
            }
            else
            {
                reservationController.updateReservation(reservation, newres);
            }

            this.Close();
            //_reservationController.reserveationList.makeItems(newDate.Date); TODO: these two should be implemented using events at those places
            //_reservationController.pendingReservationList.makeItems();
            
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