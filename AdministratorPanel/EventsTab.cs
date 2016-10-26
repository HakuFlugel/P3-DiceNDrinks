using System.Drawing;
using System.Windows.Forms;

namespace AdministratorPanel {
    public class EventsTab : TabPage {
        Form form;
        public EventsTab(Form form) {
            this.form = form;
            Text = "Events";
            
            Button newEvent = new Button {
                Height = 20,
                Width = 100,
                Text = "New Event",
                Location = new Point((form.Width / 10) * 8 + (Width / 4), 10)};

            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.Dock = DockStyle.Left;
            tlp.BackColor = Color.Transparent;
            tlp.Width = (form.Width / 10) * 8 + (newEvent.Width / 4);
            
            
            Controls.Add(newEvent);
            tlp.Controls.Add(new Button { Height = 20, Width = 100, Text = "Test :D" });
            Controls.Add(tlp);
            
        }


    }
}
