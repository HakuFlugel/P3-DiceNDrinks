using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class PendingReservationList : TableLayoutPanel
    {

        public PendingReservationList(CalendarTab CalTab)
        {
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            AutoScroll = true;
            
            //VerticalScroll = true;
            VScroll = true;
            foreach (var item in CalTab.CalDay) {
                PendingReservationItem pendingReservationItem = new PendingReservationItem(item, item.reservations);
                Controls.Add(pendingReservationItem); 
            }
        }
    }
}