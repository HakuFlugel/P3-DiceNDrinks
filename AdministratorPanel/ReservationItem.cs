using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel
{
    public class ReservationItem : Buttom
    {

        public ReservationItem(Reservation reservation)
        {
            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);

            Controls.Add(new Label{ Text = "Dinmor\n\ntests\n\ntest", AutoSize = true}); // TODO: add content from reservation
        }

    }
}