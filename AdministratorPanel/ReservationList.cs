using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class ReservationList : TableLayoutPanel
    {
        public ReservationList()
        {
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.Fixed3D;
            ColumnCount = 1;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            //AutoScroll = true;
            VScroll = true;

            for (int i = 0; i < 12; i++)
            {

                ReservationItem reservationItem = new ReservationItem();

                Controls.Add(reservationItem);

//                NiceButton reservation = new NiceButton();
//                reservation.Text = "66/6 6666\nLars | 42\nJens | 66\n\ntesttse";
//                reservation.Padding = new Padding(4);
//                //reservation.TextAlign = ContentAlignment.MiddleCenter;
//                reservation.Height = 30;
//                //pendingReservation.Height = (int)(button.Height * 3 / 1.5);
//                //pendingReservation.Anchor = AnchorStyles.Left & AnchorStyles.Right /*& AnchorStyles.Top*/;
//                reservation.Dock = DockStyle.Top;
//                reservation.Margin = new Padding(4, 2, 20, 2);
//                reservation.bgColor = Color.LightGray;
//                //reservation.AutoSize = true;
//                //reservation.AutoSizeMode = AutoSizeMode.GrowOnly;
//
//                //reservation.MakeSuperClickable((s,e) => { reservation.BackColor = Color.Red; });
//
//                Controls.Add(reservation);
            }

        }
    }
}