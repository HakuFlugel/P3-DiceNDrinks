using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class PendingReservationList : TableLayoutPanel {
        private ReservationsTab calTab;
        private Calendar cal;
        public PendingReservationList(Calendar cal, ReservationsTab calTab) {
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
            foreach (var item in calTab.calDayList.OrderBy(o => o.theDay.Date)) {
                if (item.reservations.Exists(o => o.pending == true)) {
                    PendingReservationItem pendingReservationItem = new PendingReservationItem(cal, calTab, item.theDay, item.reservations);
                    Controls.Add(pendingReservationItem);
                }
            }
        }
    }
}