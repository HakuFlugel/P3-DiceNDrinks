﻿using System;
using System.Linq;
using System.Windows.Forms;
using Shared;
using System.Globalization;
using System.Media;

namespace AdministratorPanel {
    public class EventPopupBox : FancyPopupBox {

        private NiceTextBox eventName = new NiceTextBox() {
            Width = 200,
            waterMark = "Event Name",
            Margin = new Padding(4, 10, 20, 10)
        };

        private NiceTextBox eventDescription = new NiceTextBox() {
            Width = 200,
            Height = 100,
            waterMark = "Event Description",
            Multiline = true,
            Margin = new Padding(4, 0, 20, 10)
        };

        private DateTimePicker startDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 10, 20, 10)
        };

        private DateTimePicker endDatePicker = new DateTimePicker() {
            Dock = DockStyle.Right,
            Margin = new Padding(0, 0, 20, 10)
        };

        private NiceTextBox startTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        private NiceTextBox endTimePicker = new NiceTextBox() {
            waterMark = "hh:mm"
        };

        private Label startDate = new Label() {
            Text = "Start Date"
        };

        private Label endDate = new Label() {
            Text = "End Date"
        };

        private Label timeDate = new Label() {
            Text = "Time"
        };

        private TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel {
            RowCount = 1,
            ColumnCount = 2,
            AutoSize = true,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        TableLayoutPanel innerLeftTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        TableLayoutPanel innerRightTableLayoutPanel = new TableLayoutPanel() {
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.AddRows,
            AutoSize = true
        };

        private EventsTab eventsTab;
        private Event evnt;

        public EventPopupBox(EventsTab eventsTab, Event evnt = null) {
            // name of the popup
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

            Controls.Add(headerTableLayoutPanel);

            innerLeftTableLayoutPanel.Controls.Add(eventName);
            innerLeftTableLayoutPanel.Controls.Add(eventDescription);

            innerRightTableLayoutPanel.Controls.Add(startDate);
            innerRightTableLayoutPanel.Controls.Add(startDatePicker);
            innerRightTableLayoutPanel.Controls.Add(endDate);
            innerRightTableLayoutPanel.Controls.Add(endDatePicker);
            innerRightTableLayoutPanel.Controls.Add(timeDate);
            innerRightTableLayoutPanel.Controls.Add(startTimePicker);
            innerRightTableLayoutPanel.Controls.Add(endTimePicker);

            headerTableLayoutPanel.Controls.Add(innerLeftTableLayoutPanel);
            headerTableLayoutPanel.Controls.Add(innerRightTableLayoutPanel);
            return headerTableLayoutPanel;
        }

        protected override void delete(object sender, EventArgs e) {
            if (DialogResult.Yes == MessageBox.Show("Delete Event", "Are you sure you want to delete this event?", MessageBoxButtons.YesNo)) {
                eventsTab.EventList.Remove(evnt);
                eventsTab.makeItems();
            }
        }

        protected override void save(object sender, EventArgs e) {

            DateTime expectedStartDate, expectedEndDate;
            if (!DateTime.TryParseExact(startTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedStartDate) ||
                !DateTime.TryParseExact(endTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out expectedEndDate)) {
                SystemSounds.Hand.Play();
                MessageBox.Show("Invalid Time", "The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            expectedStartDate =  startDatePicker.Value.Add(expectedStartDate.TimeOfDay);
            expectedStartDate =  endDatePicker.Value.Add(expectedStartDate.TimeOfDay);

            if (eventName.Text == null || eventDescription.Text == null) {
                SystemSounds.Hand.Play();
                MessageBox.Show("You need to add a name and description");
                return;
            }

            if (evnt == null) {
                evnt = new Event();
                eventsTab.EventList.Add(evnt);
            }

            evnt.name = eventName.Text;
            evnt.description = eventDescription.Text;

            evnt.startDate = expectedStartDate;
            evnt.endDate = expectedStartDate;

            this.Close();
            eventsTab.makeItems();
        }
    }
}
