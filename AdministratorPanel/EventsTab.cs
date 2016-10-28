using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class EventsTab : TabPage {
        Form form;
        public EventsTab(Form form) {
            this.form = form;
            Text = "Events";
            
            /*Button newEvent = new Button {
                Height = 20,
                Width = 100,
                Text = "New Event",
                Location = new Point(form.Width - (Width / 4) * 3, 10)};*/

            TableLayoutPanel headtlp = new TableLayoutPanel();
            headtlp.Dock = DockStyle.Fill;
            headtlp.BackColor = Color.Transparent;
            headtlp.RowCount = 2;
            headtlp.ColumnCount = 1;

            FlowLayoutPanel topflp = new FlowLayoutPanel();
            FlowLayoutPanel lowerflp = new FlowLayoutPanel();
            lowerflp.AutoScroll = true;

            TableLayoutPanel eList = new TableLayoutPanel();
            eList.RowCount = 1;
            eList.ColumnCount = 2;
            //FlowLayoutPanel eLoList

            topflp.Controls.Add(new Button { Height = 20, Width = 100, Text = "Test :D" });
            //lowerflp.Controls.Add(new Button { })
            
            headtlp.Controls.Add(topflp);
            headtlp.Controls.Add(lowerflp);

            //Controls.Add(newEvent);
            //headtlp.Controls.Add(new Button { Height = 20, Width = 100, Text = "Test :D" });
            Controls.Add(headtlp);
            
        }


    }
}
