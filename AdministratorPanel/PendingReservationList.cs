using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class PendingReservationList : TableLayoutPanel
    {

        public PendingReservationList(Calendar cal, CalendarTab calTab)
        {
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoScroll = true;


            //VerticalScroll = true;
            VScroll = true;
            
            foreach (var item in calTab.calDay) {
                if (item.reservations.Exists(o => o.pending)) {
                    PendingReservationItem pendingReservationItem = new PendingReservationItem(cal, item.theDay, item.reservations);
                    Controls.Add(pendingReservationItem);
                }
            }
        }
    }
}