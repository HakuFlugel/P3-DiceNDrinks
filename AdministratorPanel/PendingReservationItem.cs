using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel
{
    public class PendingReservationItem : NiceButton
    {
        public PendingReservationItem()
        {
            RowCount = 1;
            ColumnCount = 1;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Margin = new Padding(4, 4, 20, 4);
            //this.MakeSuperClickable((s, ev) => { this.BackColor = Color.Red; });


            Controls.Add(new Label{ Text = "Dinmor"});
        }
    }
}