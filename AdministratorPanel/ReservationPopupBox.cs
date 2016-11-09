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

        DateTimePicker DatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };
        NiceTextBox TimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        private CalendarTab calTab;
        private Reservation res;

        public ReservationPopupBox() {

        }

        public ReservationPopupBox(CalendarTab calTab, Reservation res = null) {
            this.calTab = calTab;
            this.res = res;
            if (res != null) {
                reservationName.Text = res.name;
                numPeople.Text = res.numPeople.ToString();
                /*TODO: better way than this?:*/
                if (res.phone != null) {
                    phoneNumber.Text = res.phone;
                }
                if(res.email != null) {
                    email.Text = res.email;
                }
                DatePicker.Value = res.time.Date;
                TimePicker.Text = res.time.ToString("HH:mm");
                
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

            lft.Controls.Add(DatePicker);
            lft.Controls.Add(TimePicker);


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
            if (!DateTime.TryParseExact(TimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate)) {

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

            string tempDate = DatePicker.Value.ToString("dd-MM-yyyy");
            string tempTime = TimePicker.Text;

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
                res.pending = true;
                cd.reservations.Add(res);
            }
            else if (newDate.Date != res.time.Date) {
                foreach (var item in calTab.calDayList) {
                    item.reservations.Remove(res);
                }
                cd.reservations.Add(res);
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
    }
}
