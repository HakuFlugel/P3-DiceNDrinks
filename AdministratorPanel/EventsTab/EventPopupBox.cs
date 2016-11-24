using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Globalization;
using System.Media;

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

        Label startDate = new Label() {
            Text = "Start Date"
        };

        Label endDate = new Label() {
            Text = "End Date"
        };

        Label timeDate = new Label() {
            Text = "Time"
        };


        private EventsTab eventsTab;
        private Event evnt;


        public EventPopupBox(EventsTab eventsTab, Event evnt = null)
        {

            Text = "Event";

            this.eventsTab = eventsTab;
            this.evnt = evnt;
            if (evnt != null) {
                try {
                    eventName.Text = evnt.name;
                    eventDescription.Text = evnt.description;
                    startDatePicker.Value = evnt.startDate;
                    endDatePicker.Value = evnt.endDate;
                    startTimePicker.Text = evnt.startDate.ToString("HH:mm");
                    endTimePicker.Text = evnt.endDate.ToString("HH:mm");
                }
                catch (ArgumentOutOfRangeException) {

                    
                }

            }
            else {
                Controls.Find("delete", true).First().Enabled = false;
            }
        }

        protected override Control CreateControls() {

            TableLayoutPanel header = new TableLayoutPanel();
            header.RowCount = 1;
            header.ColumnCount = 2;
            //header.Dock = DockStyle.Fill;
            header.AutoSize = true;
            header.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            Controls.Add(header);

            TableLayoutPanel Leftpanel = new TableLayoutPanel();
            Leftpanel.ColumnCount = 1;
            Leftpanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Leftpanel.Dock = DockStyle.Fill;

            Leftpanel.Controls.Add(eventName);
            Leftpanel.Controls.Add(eventDescription);

            TableLayoutPanel rightPanel = new TableLayoutPanel();
            rightPanel.ColumnCount = 1;
            rightPanel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            rightPanel.Dock = DockStyle.Fill;

            rightPanel.Controls.Add(startDate);
            rightPanel.Controls.Add(startDatePicker);
            rightPanel.Controls.Add(endDate);
            rightPanel.Controls.Add(endDatePicker);
            rightPanel.Controls.Add(timeDate);
            rightPanel.Controls.Add(startTimePicker);
            rightPanel.Controls.Add(endTimePicker);

            header.Controls.Add(Leftpanel);
            header.Controls.Add(rightPanel);
            return header;
        }

        protected override void delete(object sender, EventArgs e) {
            if (DialogResult.Yes == MessageBox.Show("Delete Event", "Are you sure you want to delete this event?", MessageBoxButtons.YesNo)) {
                eventsTab.Evnts.Remove(evnt);
                eventsTab.makeItems();
            }
        }

        protected override void save(object sender, EventArgs e) {

            // TODO: redo date parsing...
            // TODO: make message box text nicer
            DateTime expectedDate;
            if (!DateTime.TryParseExact(startTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate) &&
                !DateTime.TryParseExact(endTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedDate)) {
                SystemSounds.Hand.Play();
                MessageBox.Show("Invalid Time", "The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            if (eventName.Text == null || eventDescription.Text == null) {
                SystemSounds.Hand.Play();
                MessageBox.Show("You need to input a name and description");
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
