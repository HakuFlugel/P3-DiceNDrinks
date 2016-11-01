using System.Drawing;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    public class EventsTab : TabPage {
        Form form;
        public EventsTab(Form form) {
            this.form = form;
            Text = "Events";

            TableLayoutPanel headtlp = new TableLayoutPanel();
            headtlp.Dock = DockStyle.Fill;
            headtlp.BackColor = Color.Transparent;
            headtlp.RowCount = 2;
            headtlp.ColumnCount = 1;

            FlowLayoutPanel topflp = new FlowLayoutPanel();
            topflp.Dock = DockStyle.Top;
            topflp.AutoSize = true;

            TableLayoutPanel lowertlp = new TableLayoutPanel();
            lowertlp.Dock = DockStyle.Fill;
            lowertlp.AutoScroll = true;
            lowertlp.ColumnCount = 1;
            lowertlp.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            for (int i = 0; i < 20; i++) {
                EventItem ei = new EventItem(new Event());
                lowertlp.Controls.Add(ei);
            }

            Button addEvent = new Button();
            addEvent.Height = 20;
            addEvent.Width = 100;
            addEvent.Text = "Add Event";
            //addEvent.Click += new System.EventHandler(this.);

            topflp.Controls.Add(addEvent);           
            headtlp.Controls.Add(topflp);
            headtlp.Controls.Add(lowertlp);

            Controls.Add(headtlp);
            
        }


    }
}
