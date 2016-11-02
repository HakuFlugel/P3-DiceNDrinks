using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Shared;

namespace AdministratorPanel {
    public class EventsTab : TabPage {
        Form form;
        public List<Event> Events = new List<Event>();

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
            topflp.FlowDirection = FlowDirection.RightToLeft;
            topflp.AutoSize = true;

            

            EventList lowertlp = new EventList();

            for (int i = 0; i < 3; i++) {
                EventItem ei = new EventItem(new Event() { name = "Pandekage dag", description = "This is a test", startDate = new System.DateTime(2016, 11, 3, 22, 00, 00), endDate = new System.DateTime(2016, 11, 3, 23, 00, 00) });
                lowertlp.Controls.Add(ei);
            }
            foreach (var item in Events) {

                //lowertlp.Controls.Add(ei);
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
