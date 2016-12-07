using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Shared;

namespace AdministratorPanel {
    public class EventItem : NiceButton {

        public string name;
        public string description;
        public DateTime startDate;
        public DateTime endDate;

        public EventItem(EventsTab evntTab, Event evnt) {
            this.name = evnt.name;
            Name = "EventItem";
            this.description = evnt.description;
            this.startDate = evnt.startDate;
            this.endDate = evnt.endDate;
            
            RowCount = 1;
            ColumnCount = 2;
            bgColor = Color.LightGray;
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Margin = new Padding(4, 4, 20, 4);
            Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(evntTab, evnt);
                p.Show();
            };

            TableLayoutPanel left = new TableLayoutPanel();
            left.Dock = DockStyle.Top;
            left.RowCount = 2;
            left.ColumnCount = 1;
            left.AutoSize = true;
            
            left.Controls.Add(new Label { Text = name, Font = new Font("Arial", 20), Dock = DockStyle.Top, AutoSize = true });
            left.Controls.Add(new Label { Text = description, Dock = DockStyle.Top, Width = 625 });

            Controls.Add(left);
            Controls.Add(new Label { Text = "\n Start date: " + startDate.ToString("ddddd, dd. MMMM, yyyy HH:mm") + 
                                    "\n\n End date: " + endDate.ToString("ddddd, dd. MMMM, yyyy HH:mm"), Dock = DockStyle.Left, AutoSize = true });
        }
    }
}
