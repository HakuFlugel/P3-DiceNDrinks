using System.Windows.Forms;

namespace AdministratorPanel {
    public class CalendarTab : TabPage
    {
        private Calendar calendar;

        public CalendarTab() {
            Text = "Calendar";

            TableLayoutPanel outerTable = new TableLayoutPanel();
            outerTable.Dock = DockStyle.Fill;
            outerTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            outerTable.RowCount = 1;
            outerTable.ColumnCount = 2;
            Controls.Add(outerTable);

            //// Left Side
            TableLayoutPanel leftTable = new TableLayoutPanel();
            leftTable.Dock = DockStyle.Left;
            leftTable.AutoSize = true;
            leftTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //leftTable.BorderStyle = BorderStyle.Fixed3D;
            outerTable.Controls.Add(leftTable);

            calendar = new Calendar();
            //calendar.Dock = DockStyle.Top;
            //calendar.Anchor = AnchorStyles.Top;
            leftTable.Controls.Add(calendar);

            PendingReservationList b = new PendingReservationList();
            leftTable.Controls.Add(b);

            //// Right side
            TableLayoutPanel rightTable = new ReservationList();
            //rightTable.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            //rightTable.Dock = DockStyle.Fill;
            //rightTable.AutoSize = true;
            //rightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //rightTable.BorderStyle = BorderStyle.Fixed3D;
            outerTable.Controls.Add(rightTable);




            /*

*/
        }
    }
}
