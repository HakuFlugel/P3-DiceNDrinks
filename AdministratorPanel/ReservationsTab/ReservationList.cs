using System;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class ReservationList : TableLayoutPanel {

        private Calendar calendar;
        public CalendarDay cd;
        private ReservationController reservationController;

        public ReservationList(Calendar calendar, ReservationController reservationController) {
            this.calendar = calendar;
            this.reservationController = reservationController;

            calendar.DateChanged += (sender, args) => { makeItems(args.Start); };
            makeItems(calendar.SelectionStart);

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoSize = true;
            HorizontalScroll.Maximum = 0;
            AutoScroll = true;

            reservationController.ReservationUpdated += (sender, args) => {
                updateCurrentDay();
            };
        }

        public void updateCurrentDay() {
            makeItems(calendar.SelectionStart.Date);
        }

        public void makeItems(DateTime day) {
            Controls.Clear();

            cd = reservationController.reservationsCalendar.Find(o => o.theDay.Date == day.Date);
            if (cd == null) {
                return;
            }
            calendar.SelectionStart = cd.theDay.Date;

            //SuspendLayout();
            foreach (var res in cd.reservations.OrderBy(o => o.time.TimeOfDay).OrderBy(o => o.state == Reservation.State.Accepted)) {
                ReservationItem reservationItem = new ReservationItem(reservationController, res);

                Controls.Add(reservationItem);
            }
            //ResumeLayout();
        }

        public void lockReservations(bool isChecked) {
            if (cd == null) {
                cd = new CalendarDay(reservationController.autoAcceptSettings.defaultAcceptPercentage, reservationController.autoAcceptSettings.defaultAcceptMaxPeople) { theDay = calendar.SelectionStart, isFullChecked = false };
                reservationController.reservationsCalendar.Add(cd);
            }
            cd.isFullChecked = isChecked;
            //cd.isFullChecked = (Parent.Parent.Parent as ReservationTab).reservationFull.Checked;
            //cd.isFullChecked = calTab.reservationFull.Checked;
        }
    }
}