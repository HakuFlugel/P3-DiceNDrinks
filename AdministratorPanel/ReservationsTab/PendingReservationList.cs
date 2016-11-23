using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class PendingReservationList : TableLayoutPanel {
        private Calendar cal;
        private ReservationController reservationController;
        private ReservationTab calTab;

        public PendingReservationList(Calendar cal, ReservationTab calTab, ReservationController reservationController) {
            this.reservationController = reservationController;
            this.cal = cal;
            this.calTab = calTab;

            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoScroll = true;
            VScroll = true;

            //TODO: is this implementation too hacky? maybe do it in item as mentioned in another todo
            reservationController.ReservationAdded += (sender, args) =>
            {
                makeItems();
            };
            reservationController.ReservationRemoved += (sender, args) =>
            {
                makeItems();
            };
            reservationController.ReservationUpdated += (sender, args) =>
            {
                makeItems();
            };

            makeItems();
        }
        public void makeItems() {
            Controls.Clear();
            foreach (var item in reservationController.reservationsCalendar.OrderBy(o => o.theDay.Date)) {
                if (item.reservations.Exists(o => o.pending)) {
                    PendingReservationItem pendingReservationItem = new PendingReservationItem(cal, calTab, item.theDay, item.reservations);
                    Controls.Add(pendingReservationItem);
                }
            }
        }
    }
}