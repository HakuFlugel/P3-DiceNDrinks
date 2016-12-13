using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using Shared;
using System.Globalization;
using System.Media;
using Newtonsoft.Json;

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
            SubscriberController();
        }

        private void SubscriberController() {
            eventName.TextChanged += (s, e) => {
                hasBeenChanged = (evnt != null) ? evnt.name != eventName.Text ? true : false : true;
            };
            eventDescription.TextChanged += (s, e) => {
                hasBeenChanged = (evnt != null) ? evnt.description != eventDescription.Text ? true : false : true;
            };
            startDatePicker.ValueChanged += (s, e) => {
                hasBeenChanged = true;
            };
            endDatePicker.ValueChanged += (s, e) => {
                hasBeenChanged = true;
            };
            endTimePicker.TextChanged += (s, e) => {
                hasBeenChanged = true;
            };
            startTimePicker.TextChanged += (s, e) => {
                hasBeenChanged = true;
            };
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
            if (DialogResult.Yes == NiceMessageBox.Show("Delete Event", "Are you sure you want to delete this event?", MessageBoxButtons.YesNo)) {
                string response = ServerConnection.sendRequest("/submitEvent.aspx",
                    new NameValueCollection() {
                        {"Action", "delete"},
                        {"Event", JsonConvert.SerializeObject(evnt)}
                    }
                );

                Console.WriteLine(response);

                if (response != "deleted")
                {
                    return;
                }

                eventsTab.EventList.Remove(evnt);
                eventsTab.makeItems();
            }
        }

        

        protected override void save(object sender, EventArgs e) {

            DateTime startDate, endDate;
            if (!DateTime.TryParseExact(startTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) ||
                !DateTime.TryParseExact(endTimePicker.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate)) {
                SystemSounds.Hand.Play();
                MessageBox.Show("Invalid Time", "The time input box(es) is incorrect please check, if they have the right syntax(hh:mm). Example: 23:59");
                return;
            }
            startDate =  startDatePicker.Value.Add(startDate.TimeOfDay);
            endDate =  endDatePicker.Value.Add(startDate.TimeOfDay);

            if (eventName.Text == null || eventDescription.Text == null) {
               
                NiceMessageBox.Show("You need to input a name and description");
                return;
            }

            bool isNew = evnt == null;
            if (isNew) {
                evnt = new Event();
                eventsTab.EventList.Add(evnt);
            }

            evnt.name = eventName.Text;
            evnt.description = eventDescription.Text;

            if(endDate < startDate) {
                NiceMessageBox.Show("Take a look at the start time vs the end time, or the start date vs the end date" + 
                    Environment.NewLine + "There is something wrong here" + Environment.NewLine + "Start date: " + startDate.ToString() + 
                    Environment.NewLine + "End date: " + endDate.ToString());
                return;
            }


            evnt.startDate = startDate;
            evnt.endDate = endDate;
            try {
                string response = ServerConnection.sendRequest("/submitEvent.aspx",
                new NameValueCollection() {
                    {"Action", isNew ? "add" : "update"},
                    {"Event", JsonConvert.SerializeObject(evnt)}
                }
            );

                Console.WriteLine(response);

                if (response.Split(' ')[0] != (isNew ? "added" : "updated")) {
                    return;
                }

                if (isNew) {
                    int.TryParse(response.Split(' ')[1], out evnt.id);
                }
            }
            catch (Exception) {
                NiceMessageBox.Show("Failed to save to the server, changes will not be send to the server", "Server connection problem");
                return;
            }
            base.save(sender,e);
            this.Close();
            eventsTab.makeItems();
        }
    }
}
