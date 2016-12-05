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
        private FormProgressBar probar;

        public EventsTab(FormProgressBar probar) {
            this.probar = probar;

            Load();
            probar.addToProbar();                               //For progress bar. 1
            Text = "Events";

            TableLayoutPanel headTable = new TableLayoutPanel();
            headTable.Dock = DockStyle.Fill;
            probar.addToProbar();                               //For progress bar. 2
            headTable.RowCount = 2;
            headTable.ColumnCount = 1;

            FlowLayoutPanel topFlowPanel = new FlowLayoutPanel();
            topFlowPanel.Dock = DockStyle.Top;
            topFlowPanel.FlowDirection = FlowDirection.RightToLeft;
            topFlowPanel.AutoSize = true;

            probar.addToProbar();                               //For progress bar. 3

            makeItems();
            probar.addToProbar();                               //For progress bar. 4
            Button addEventButton = new Button();
            addEventButton.Height = 20;
            addEventButton.Width = 100;
            addEventButton.Text = "Add Event";
            probar.addToProbar();                               //For progress bar. 5
            addEventButton.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
                p.Show();
            };

            topFlowPanel.Controls.Add(addEventButton);
            probar.addToProbar();                               //For progress bar. 6          
            headTable.Controls.Add(topFlowPanel);
            probar.addToProbar();                               //For progress bar. 7
            headTable.Controls.Add(lowerTable);
            probar.addToProbar();                               //For progress bar. 8

            Controls.Add(headTable);
            probar.addToProbar();                               //For progress bar. 9
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

            if (Evnts == null) {
                Evnts = new List<Event>();
            }
        }
    }
}
