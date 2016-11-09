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
        EventList lowertlp = new EventList();

        public EventsTab() {
            Load();

            Text = "Events";

            TableLayoutPanel headtlp = new TableLayoutPanel();
            headtlp.Dock = DockStyle.Fill;
            headtlp.BackColor = Color.Transparent;
            headtlp.RowCount = 2;
            headtlp.ColumnCount = 1;

            FlowLayoutPanel topflp = new FlowLayoutPanel();
            topflp.Dock = DockStyle.Top;
            topflp.FlowDirection = FlowDirection.RightToLeft;
            topflp.AutoSize = true;

            makeItems();

            Button addEvent = new Button();
            addEvent.Height = 20;
            addEvent.Width = 100;
            addEvent.Text = "Add Event";
            addEvent.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
                p.Show();
            };

            topflp.Controls.Add(addEvent);           
            headtlp.Controls.Add(topflp);
            headtlp.Controls.Add(lowertlp);

            Controls.Add(headtlp);
            
        }

        public void makeItems() {
            lowertlp.Controls.Clear();

            foreach (var item in Evnts.OrderBy((Event e) => e.startDate)) {
                lowertlp.Controls.Add(new EventItem(this, item));
            }
        }

        public override void Save() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Event>));
            using (StreamWriter textWriter = new StreamWriter(@"Scourses/fix.xml")) {
                serializer.Serialize(textWriter, Evnts);
            }
        }

        public override void Load() {
            //XmlDeclaration deserializer = new XmlDeclaration();
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Event>));
            using (FileStream fileReader = new FileStream(@"Scourses/fix.xml", FileMode.OpenOrCreate)) {
                try {
                    Evnts = deserializer.Deserialize(fileReader) as List<Event>;

                }
                catch (Exception) { }
            }   
        }
    }
}
