using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class ReservationList : TableLayoutPanel
    {
        private Calendar calendar;
        public CalendarDay cd;
        private ReservationController reservationController;

        public ReservationList(Calendar calendar, ReservationController reservationController)
        {
            this.calendar = calendar;
            this.reservationController = reservationController;

            calendar.DateSelected += (sender, args) => { makeItems(args.Start);};
            makeItems(calendar.SelectionStart);

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoSize = true;
            HorizontalScroll.Maximum = 0;
            AutoScroll = true;
            
            
        }

        //TODO: call on change...
        public void makeItems(DateTime day)
        {
            Controls.Clear();
            
            cd = reservationController.reservationsCalendar.Find(o => o.theDay.Date == day.Date);
            if (cd == null) {
                return;
            }
            calendar.SelectionStart = cd.theDay.Date;
//TODO: move to event in reservationtab or should it be part of this class instead?
//            calTab.reserveSpaceValue = 0;
//            foreach (var item in cd.reservations.FindAll(o => o.pending == false)) {
//                calTab.reserveSpaceValue += item.numPeople;
//            }
//            if (calTab.reserveSpaceValue < 100) {
//                calTab.reserveSpace.Value = calTab.reserveSpaceValue;
//            }
//            else {
//                calTab.reserveSpace.Value = 100;
//                MessageBox.Show("The reservation max count is exceeded!");
//            }
//            calTab.reserveSpaceText.Text = calTab.reserveSpaceValue.ToString() + " / 100";

//            calTab.reservationFull.Checked = cd.isFullChecked;

            SuspendLayout();
            foreach (var res in cd.reservations.OrderBy(o => o.time.TimeOfDay).OrderBy(o => !o.pending)) {
                ReservationItem reservationItem = new ReservationItem(reservationController, res);

                Controls.Add(reservationItem);
            }
            ResumeLayout();
           
        }

        public void lockReservations(bool isChecked) {
            if (cd == null) {
                cd = new CalendarDay { theDay = calendar.SelectionStart, isFullChecked = false };
                reservationController.reservationsCalendar.Add(cd);
            }
            cd.isFullChecked = isChecked;
            //cd.isFullChecked = (Parent.Parent.Parent as ReservationTab).reservationFull.Checked;
            //cd.isFullChecked = calTab.reservationFull.Checked;
        }
    }
}