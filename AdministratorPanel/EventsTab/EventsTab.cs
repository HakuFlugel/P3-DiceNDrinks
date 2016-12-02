using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Shared;
using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace AdministratorPanel {
    public class EventsTab : AdminTabPage {
        public List<Event> Evnts = new List<Event>();
        EventList lowerTable = new EventList();

        public EventsTab() {
            Load();

            Text = "Events";

            TableLayoutPanel headTable = new TableLayoutPanel();
            headTable.Dock = DockStyle.Fill;
            //headTable.BackColor = Color.Transparent;
            headTable.RowCount = 2;
            headTable.ColumnCount = 1;

            FlowLayoutPanel topFlowPanel = new FlowLayoutPanel();
            topFlowPanel.Dock = DockStyle.Top;
            topFlowPanel.FlowDirection = FlowDirection.RightToLeft;
            topFlowPanel.AutoSize = true;

            makeItems();

            Button addEventButton = new Button();
            addEventButton.Height = 20;
            addEventButton.Width = 100;
            addEventButton.Text = "Add Event";
            addEventButton.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
                p.Show();
            };

            topFlowPanel.Controls.Add(addEventButton);           
            headTable.Controls.Add(topFlowPanel);
            headTable.Controls.Add(lowerTable);

            Controls.Add(headTable);
            
        }

        public void makeItems() {
            lowerTable.Controls.Clear();

            foreach (var item in Evnts.OrderBy((Event e) => e.startDate)) {
                lowerTable.Controls.Add(new EventItem(this, item));
            }
        }

        //TODO: change xml to json
        public override void Save() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Event>));
            using (StreamWriter textWriter = new StreamWriter(@"fix.xml")) {
                serializer.Serialize(textWriter, Evnts);
            }
        }

        public override void Load() {
            //XmlDeclaration deserializer = new XmlDeclaration();
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Event>));
            try
            {
                using (FileStream fileReader = new FileStream(@"fix.xml", FileMode.OpenOrCreate)) {
                
                    Evnts = deserializer.Deserialize(fileReader) as List<Event>;

                }
            }
            catch (Exception) { }
        }
    }
}
