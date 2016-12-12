using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Shared;
using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AdministratorPanel {
    public class EventsTab : AdminTabPage {
        public List<Event> EventList = new List<Event>();
        private EventList lowerTable = new EventList();
        private FormProgressBar probar;

        private TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel() {
            Dock = DockStyle.Fill,                        
            RowCount = 2,
            ColumnCount = 1,
        };

        private FlowLayoutPanel innerTopFlowLayoutPanel = new FlowLayoutPanel() {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
        };

        private Button addEventButton = new Button() {
            Height = 20,
            Width = 100,
            Text = "Add Event",
        };

        public EventsTab(FormProgressBar probar) {
            //name of the tab
            Text = "Events";
            this.probar = probar;

            Load();
            probar.addToProbar();                               //For progress bar. 1

            makeItems();
            probar.addToProbar();                               //For progress bar. 2

            addEventButton.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
            };
            probar.addToProbar();                               //For progress bar. 3

            innerTopFlowLayoutPanel.Controls.Add(addEventButton);
            probar.addToProbar();                               //For progress bar. 4
                      
            headerTableLayoutPanel.Controls.Add(innerTopFlowLayoutPanel);
            probar.addToProbar();                               //For progress bar. 5

            headerTableLayoutPanel.Controls.Add(lowerTable);
            probar.addToProbar();                               //For progress bar. 6

            Controls.Add(headerTableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 7
        }

        public void makeItems() {
            lowerTable.Controls.Clear();

            foreach (var item in EventList.OrderBy((Event e) => e.startDate)) {
                lowerTable.Controls.Add(new EventItem(this, item));
            }
        }

        public override void Save() {

            var jsonEvents = JsonConvert.SerializeObject(EventList);
            Directory.CreateDirectory("Sources");
            using (StreamWriter textWriter = new StreamWriter(@"Sources/EventList.json")) {
                foreach (var item in jsonEvents) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {

            string loadStringCategory;
            if (File.Exists(@"Sources/EventList.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/EventList.json")) {
                    loadStringCategory = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (loadStringCategory != null) {
                    EventList = JsonConvert.DeserializeObject<List<Event>>(loadStringCategory);
                } else {
                    EventList = new List<Event>();
                }
            }
        }
    }

    public class CopyOfEventsTab : AdminTabPage {
        public List<Event> EventList = new List<Event>();
        private EventList lowerTable = new EventList();
        private FormProgressBar probar;

        private TableLayoutPanel headerTableLayoutPanel = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1,
        };

        private FlowLayoutPanel innerTopFlowLayoutPanel = new FlowLayoutPanel() {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.RightToLeft,
            AutoSize = true,
        };

        private Button addEventButton = new Button() {
            Height = 20,
            Width = 100,
            Text = "Add Event",
        };

        public CopyOfEventsTab(FormProgressBar probar) {
            //name of the tab
            Text = "Events";
            this.probar = probar;

            Load();
            probar.addToProbar();                               //For progress bar. 1

            makeItems();
            probar.addToProbar();                               //For progress bar. 2

            addEventButton.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
            };
            probar.addToProbar();                               //For progress bar. 3

            innerTopFlowLayoutPanel.Controls.Add(addEventButton);
            probar.addToProbar();                               //For progress bar. 4

            headerTableLayoutPanel.Controls.Add(innerTopFlowLayoutPanel);
            probar.addToProbar();                               //For progress bar. 5

            headerTableLayoutPanel.Controls.Add(lowerTable);
            probar.addToProbar();                               //For progress bar. 6

            Controls.Add(headerTableLayoutPanel);
            probar.addToProbar();                               //For progress bar. 7
        }

        public void makeItems() {
            lowerTable.Controls.Clear();

            foreach (var item in EventList.OrderBy((Event e) => e.startDate)) {
                lowerTable.Controls.Add(new EventItem(this, item));
            }
        }

        public override void Save() {

            var jsonEvents = JsonConvert.SerializeObject(EventList);
            Directory.CreateDirectory("Sources");
            using (StreamWriter textWriter = new StreamWriter(@"Sources/EventList.json")) {
                foreach (var item in jsonEvents) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public override void Load() {

            string loadStringCategory;
            if (File.Exists(@"Sources/EventList.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/EventList.json")) {
                    loadStringCategory = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (loadStringCategory != null) {
                    EventList = JsonConvert.DeserializeObject<List<Event>>(loadStringCategory);
                } else {
                    EventList = new List<Event>();
                }
            }
        }
    }
}
