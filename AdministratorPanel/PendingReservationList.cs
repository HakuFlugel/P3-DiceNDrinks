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
            //AutoScroll = true;
            //VerticalScroll = true;
            VScroll = true;
            foreach (var item in CalTab.reservations) {
                PendingReservationItem pendingReservationItem = new PendingReservationItem(item.reservations);
            }
            
            for (int i = 0; i < 12; i++)
            {
                //                pendingReservation.bgColor = Color.LightGray;
                //                //pendingReservation.AutoSize = true;
                //                //pendingReservation.AutoSizeMode = AutoSizeMode.GrowOnly;
                //
                //
                //                //pendingReservation.MakeSuperClickable((s,e) => { pendingReservation.BackColor = Color.Red; });
                //
                //                Controls.Add(pendingReservation);
            }
        }

    }
}