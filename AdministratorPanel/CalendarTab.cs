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


            // Left Side
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

            FlowLayoutPanel b = new FlowLayoutPanel();
            b.Text = "Something";
            b.Dock = DockStyle.Fill;
            b.BorderStyle = BorderStyle.Fixed3D;
            b.FlowDirection = FlowDirection.TopDown;
            b.WrapContents = false;
            b.AutoScroll = true;
            leftTable.Controls.Add(b);
            //b.scroll

            for (int i = 0; i < 100; i++)
            {
                Button button = new Button();
                button.Text = "66/6 6666\nLars | 42\nJens | 66";
                //button.Height = 666;
                button.Height = (int)(button.Height * 3 / 1.5);
                //button.Anchor = AnchorStyles.Left & AnchorStyles.Right & AnchorStyles.Top;
                //button.Dock = DockStyle.Fill;
                //button.AutoSizeMode = AutoSizeMode.GrowOnly;

                b.Controls.Add(button);
                button.Width = button.Parent.ClientSize.Width - 20;
                TextBox t = new TextBox();


            }

            //b.Anchor = AnchorStyles.Top & AnchorStyles.Left & AnchorStyles.Right & AnchorStyles.Bottom;



            // Right side
            TableLayoutPanel rightTable = new TableLayoutPanel();
            rightTable.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            rightTable.Dock = DockStyle.Fill;
            //rightTable.AutoSize = true;
            //rightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            rightTable.BorderStyle = BorderStyle.Fixed3D;
            outerTable.Controls.Add(rightTable);




            /*

*/
        }
    }
}
