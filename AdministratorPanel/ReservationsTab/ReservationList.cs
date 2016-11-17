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
        private ReservationsTab calTab;
        public CalendarDay cd;

        public ReservationList(Calendar calendar, ReservationsTab calTab)
        {
            this.calendar = calendar;
            this.calTab = calTab;

            calendar.DateSelected += (sender, args) => { this.makeItems(args.Start);};
            makeItems(calendar.SelectionStart);

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoSize = true;
            HorizontalScroll.Maximum = 0;
            AutoScroll = true;
            
            
        }

        public void makeItems(DateTime day)
        {
            Controls.Clear();
            
            cd = calTab.calDayList.Find(o => o.theDay.Date == day.Date);
            if (cd == null) {
                return;
            }
            calendar.SelectionStart = cd.theDay.Date;

            calTab.reserveSpaceValue = 0;
            foreach (var item in cd.reservations.FindAll(o => o.pending == false)) {
                calTab.reserveSpaceValue += item.numPeople;
            }
            if (calTab.reserveSpaceValue < 100) {
                calTab.reserveSpace.Value = calTab.reserveSpaceValue;
            }
            else {
                calTab.reserveSpace.Value = 100;
                MessageBox.Show("The reservation max count is exceeded!");
            }
            calTab.reserveSpaceText.Text = calTab.reserveSpaceValue.ToString() + " / 100";

            calTab.reservationFull.Checked = cd.isFullChecked;

            SuspendLayout();
            foreach (var res in cd.reservations.OrderBy(o => o.time.TimeOfDay).OrderBy(o => !o.pending)) {
                ReservationItem reservationItem = new ReservationItem(calTab, res);

                Controls.Add(reservationItem);
            }
            ResumeLayout();
           
        }

        public void lockReservations(object sender, EventArgs e) {
            if (cd == null) {
                cd = new CalendarDay { theDay = calendar.SelectionStart, isFullChecked = false };
                calTab.calDayList.Add(cd);
            }
            cd.isFullChecked = calTab.reservationFull.Checked;
        }
    }
}