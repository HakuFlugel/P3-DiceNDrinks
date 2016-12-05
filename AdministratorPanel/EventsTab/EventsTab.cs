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
        private FormProgressBar probar;

        public EventsTab(FormProgressBar probar) {
            this.probar = probar;

            Load();
            probar.addToProbar();                               //For progress bar. 1
            Text = "Events";

            TableLayoutPanel headtlp = new TableLayoutPanel();
            headtlp.Dock = DockStyle.Fill;
            probar.addToProbar();                               //For progress bar. 2
            //headtlp.BackColor = Color.Transparent;
            headtlp.RowCount = 2;
            headtlp.ColumnCount = 1;

            FlowLayoutPanel topflp = new FlowLayoutPanel();
            topflp.Dock = DockStyle.Top;
            topflp.FlowDirection = FlowDirection.RightToLeft;
            topflp.AutoSize = true;
            probar.addToProbar();                               //For progress bar. 3
            makeItems();
            probar.addToProbar();                               //For progress bar. 4
            Button addEvent = new Button();
            addEvent.Height = 20;
            addEvent.Width = 100;
            addEvent.Text = "Add Event";
            probar.addToProbar();                               //For progress bar. 5
            addEvent.Click += (s, e) => {
                EventPopupBox p = new EventPopupBox(this);
                p.Show();
            };

            topflp.Controls.Add(addEvent);
            probar.addToProbar();                               //For progress bar. 6         
            headtlp.Controls.Add(topflp);
            probar.addToProbar();                               //For progress bar. 7
            headtlp.Controls.Add(lowertlp);
            probar.addToProbar();                               //For progress bar. 8

            Controls.Add(headtlp);
            probar.addToProbar();                               //For progress bar. 9

        }

        public void makeItems() {
            lowertlp.Controls.Clear();

            foreach (var item in Evnts.OrderBy((Event e) => e.startDate)) {
                lowertlp.Controls.Add(new EventItem(this, item));
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
            using (FileStream fileReader = new FileStream(@"fix.xml", FileMode.OpenOrCreate)) {
                try {
                    Evnts = deserializer.Deserialize(fileReader) as List<Event>;

                }
                catch (Exception) { }
            }   
        }
    }
}
