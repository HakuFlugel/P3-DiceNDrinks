using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace AdministratorPanel {
    class EventPopupBox : FancyPopupBox {

        NiceTextBox eventName = new NiceTextBox() {
            Width = 200,
            waterMark = "Event Name",
            Margin = new Padding(4, 10, 20, 10)
        };
        NiceTextBox eventDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Event Description",
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10)
        };

        DateTimePicker startDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };
        NiceTextBox startTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        DateTimePicker endDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 20, 10)
        };
        NiceTextBox endTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };
        private EventsTab eventsTab;
        private Event evnt;

        public EventPopupBox() {

        }

        public EventPopupBox(EventsTab eventsTab, Event evnt = null) {
            this.eventsTab = eventsTab;
            this.evnt = evnt;
            eventName.Text = evnt.name;
            eventDescription.Text = evnt.description;
            startDatePicker.Value = evnt.startDate;
            endDatePicker.Value = evnt.endDate;
            startTimePicker.Text = evnt.startDate.ToString("hh:mm");
            endTimePicker.Text = evnt.endDate.ToString("hh:mm");
            
        }

        protected override Control CreateControls() {

            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            TableLayoutPanel rght = new TableLayoutPanel();
            rght.ColumnCount = 1;
            rght.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rght.Dock = DockStyle.Fill;

            rght.Controls.Add(eventName);
            rght.Controls.Add(eventDescription);

            TableLayoutPanel lft = new TableLayoutPanel();
            lft.ColumnCount = 1;
            lft.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            lft.Dock = DockStyle.Fill;

            lft.Controls.Add(startDatePicker);
            lft.Controls.Add(endDatePicker);
            lft.Controls.Add(startTimePicker);
            lft.Controls.Add(endTimePicker);

            header.Controls.Add(rght);
            header.Controls.Add(lft);
            return header;
        }

        protected override void delete(object sender, EventArgs e) {
            
        }

        protected override void save(object sender, EventArgs e) {
            if (evnt == null) {
                evnt = new Event();
                eventsTab.Evnts.Add(evnt);
            }
            //evnt.description = 

            //eventsTab.
        }
    }
}
