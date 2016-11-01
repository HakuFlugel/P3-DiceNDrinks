using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class PendingReservationList : TableLayoutPanel
    {

        public PendingReservationList()
        {
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoScroll = true;
            //VerticalScroll = true;
            VScroll = true;

            for (int i = 0; i < 12; i++)
            {
                PendingReservationItem pendingReservationItem = new PendingReservationItem();

                Controls.Add(pendingReservationItem);

//                NiceButton pendingReservation = new NiceButton();
//                pendingReservation.Text = "66/6 6666\nLars | 42\nJens | 66\n\ntesttse";
//                pendingReservation.Padding = new Padding(4);
//                //pendingReservation.TextAlign = ContentAlignment.MiddleCenter;
//                //pendingReservation.Height = 666;
//                //pendingReservation.Height = (int)(button.Height * 3 / 1.5);
//                //pendingReservation.Anchor = AnchorStyles.Left & AnchorStyles.Right /*& AnchorStyles.Top*/;
//                pendingReservation.Dock = DockStyle.Top;
//                pendingReservation.Margin = new Padding(4, 2, 20, 2);
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