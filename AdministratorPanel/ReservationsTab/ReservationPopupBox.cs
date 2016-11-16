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


        private ReservationTab calTab;
        private Reservation res;

        public ReservationPopupBox() {

        }

        public ReservationPopupBox(ReservationTab calTab, Reservation res = null) {
            this.calTab = calTab;
            this.res = res;
            if (res != null) {
                try {
                    reservationName.Text = res.name;
                    numPeople.Text = res.numPeople.ToString();
                    /*TODO: better way than this?:*/
                    if (res.phone != null) {
                        phoneNumber.Text = res.phone;
                    }
                    if (res.email != null) {
                        email.Text = res.email;
                    }
                    datePicker.Value = res.time.Date;
                    timePicker.Text = res.time.ToString("HH:mm");
                    pendingSet.Checked = res.pending;
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
            if (DialogResult.Yes == MessageBox.Show("Delete Reservation", "Are you sure you want to delete this reservation?", MessageBoxButtons.YesNo)) {
                foreach (var item in calTab.calDayList) {
                    item.reservations.Remove(res);
                }
                //calTab.calDay.Find(o => o.theDay.Date == res.time.Date);

                calTab.reserveationList.makeItems(DateTime.Today.Date);
                calTab.pendingReservationList.makeItems();
            }
        }

        protected override void save(object sender, EventArgs e) {

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

            

            CalendarDay cd = calTab.calDayList.Find(o => o.theDay.Date == newDate.Date);

            if (cd == null) {
                cd = new CalendarDay { theDay = newDate.Date };
                calTab.calDayList.Add(cd);
            }

            if (res == null) {
                res = new Reservation();
                res.created = DateTime.Now;
                cd.reservations.Add(res);
            }
            else if (newDate.Date != res.time.Date) {
                foreach (var item in calTab.calDayList) {
                    item.reservations.Remove(res);
                }
                cd.reservations.Add(res);
            }

            if (pendingSet.Checked == true) {
                res.pending = true;
            }
            else {
                res.pending = false;
            }
            res.name = reservationName.Text;
            int.TryParse(numPeople.Text, out res.numPeople);
            res.phone = phoneNumber.Text;
            res.email = email.Text;
            res.time = newDate;

            //foreach (var item in calTab.calDay) {
            //    if (item.theDay.Date == newStartDate.Date) {
            //    }
            //    else {
            //    }
            //}

            this.Close();
            calTab.reserveationList.makeItems(newDate.Date);
            calTab.pendingReservationList.makeItems();
            
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




/*
 public string email {
            get {
                return email;
            }
            set {
                const string validLocalSymbols = ".-"; // !#$%&'*+-/=?^_`{|}~      quoted og evt. escaped "(),:;<>@[]
                const string validDomainSymbols = ".-";



                string[] emailParts = value.Split('@');

                if (emailParts.Length != 2)
                    throw new FormatException("Email address must contain exactly one '@'");

                if (emailParts[0][0] == '.' || emailParts[0][emailParts.Length - 1] == '.' || emailParts[1][0] == '.' || emailParts[1][emailParts.Length - 1] == '.')
                    throw new FormatException("Email address local- or domain-part can't start or end with a '.'");

                if (!emailParts[1].Contains('.'))
                    throw new FormatException("Email adress domain part must contain atleast 1 '.'. ie. @domain.tld");

                if (value.Contains(".."))
                    throw new FormatException("Email address may not contain consecutive '.'s, ie. '..'.");

                foreach (char ch in emailParts[0]) {
                    if (!Char.IsLetterOrDigit(ch) && !validLocalSymbols.Contains(ch)) 
                        throw new FormatException($"Email address local-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols "{validLocalSymbols}"");
                }

                foreach (var ch in emailParts[1]) {
                    if (!Char.IsLetterOrDigit(ch) && !validDomainSymbols.Contains(ch))
                        throw new FormatException($"Email address domain-part contains invalid character '{ch}'. Can only contain letters, numbers and the symbols "{validDomainSymbols}"");
                }



                _email = value;

            }
        }
*/
