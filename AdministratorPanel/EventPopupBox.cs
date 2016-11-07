using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;

namespace AdministratorPanel {
    class EventPopupBox : FancyPopupBox {

        NiceTextBox eventName = new NiceTextBox() {
            Width = 200,
            waterMark = "Event Name",
            Margin = new Padding(4, 10, 20, 10) };
        NiceTextBox eventDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Event Description",
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10) };

        DateTimePicker startDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10) };
        NiceTextBox startTimePicker = new NiceTextBox() {
            waterMark = "hh:mm" };

        DateTimePicker endDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 20, 10) };
        NiceTextBox endTimePicker = new NiceTextBox() {
            waterMark = "hh:mm" };

        private EventsTab eventsTab;
        private Event evnt;

        public EventPopupBox() { }

        public EventPopupBox(EventsTab eventsTab, Event evnt = null) {
            this.eventsTab = eventsTab;
            this.evnt = evnt;
            if (evnt != null) {
                eventName.Text = evnt.name;
                eventDescription.Text = evnt.description;
                startDatePicker.Value = evnt.startDate;
                endDatePicker.Value = evnt.endDate;
                startTimePicker.Text = evnt.startDate.ToString("HH:mm");
                endTimePicker.Text = evnt.endDate.ToString("HH:mm");
            }
            else {
                Controls.Find("delete", true).First().Enabled = false;
            }
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
            if (DialogResult.Yes == MessageBox.Show("Delete Event", "Are you sure you want to delete this event?", MessageBoxButtons.YesNo)) {
                eventsTab.Evnts.Remove(evnt);
                eventsTab.makeItems();
            }
        }

        protected override void save(object sender, EventArgs e) {
            base.save(sender, e);
            DateTime expectedDate;
            if (!DateTime.TryParseExact(startTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate) ||
                !DateTime.TryParseExact(endTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate)) {

                MessageBox.Show("The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            if (eventName.Text == null || eventDescription.Text == null) {
                MessageBox.Show("You need to input a name AND a description");
                return;
            }
            if (evnt == null) {
                evnt = new Event();
                eventsTab.Evnts.Add(evnt);
            }
            evnt.name = eventName.Text;
            evnt.description = eventDescription.Text;
            string tempDate = startDatePicker.Value.ToString("dd-MM-yyyy");
            string tempTime = startTimePicker.Text;

            /*COPY PASTE(SOME OF IT!!)*/
            DateTime newStartDate = DateTime.ParseExact(tempDate + " " + tempTime + ":00", "dd-MM-yyyy HH:mm:00",
                                       CultureInfo.InvariantCulture);
            /*END OF COPY PASTE*/

            tempDate = endDatePicker.Value.ToString("dd-MM-yyyy");
            tempTime = endTimePicker.Text;

            DateTime newEndDate = DateTime.ParseExact(tempDate + " " + tempTime + ":00", "dd-MM-yyyy HH:mm:00",
                           CultureInfo.InvariantCulture);

            evnt.startDate = newStartDate;
            evnt.endDate = newEndDate;

            this.Close();
            eventsTab.makeItems();
        }
    }
}
