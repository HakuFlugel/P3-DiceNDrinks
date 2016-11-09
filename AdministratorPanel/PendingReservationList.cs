using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class PendingReservationList : TableLayoutPanel {
        private CalendarTab calTab;
        private Calendar cal;
        public PendingReservationList(Calendar cal, CalendarTab calTab) {
            this.calTab = calTab;
            this.cal = cal;

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoScroll = true;
            VScroll = true;

            makeItems();
        }
        public void makeItems() {
            Controls.Clear();
            foreach (var item in calTab.calDayList) {
                if (item.reservations.Exists(o => o.pending)) {
                    PendingReservationItem pendingReservationItem = new PendingReservationItem(cal, calTab, item.theDay, item.reservations);
                    Controls.Add(pendingReservationItem);
                }
            }
        }
    }
}