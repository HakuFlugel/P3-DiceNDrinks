﻿using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Shared;
using System;

namespace AdministratorPanel {
    public class EventsTab : AdminTabPage {
        private List<Event> Evnts = new List<Event>();

        public EventsTab() {
            /*Test below this*/
            Evnts.Add(new Event() {
                name = "Pandekage dagv2",
                description = "This is a test",
                startDate = new DateTime(2016, 11, 3, 22, 00, 00),
                endDate = new DateTime(2016, 11, 3, 23, 00, 00)
            });
            Evnts.Add(new Event() {
                name = "Pandekage dag",
                description = "This is a test",
                startDate = new DateTime(2016, 11, 3, 19, 00, 00),
                endDate = new DateTime(2016, 11, 3, 23, 30, 00)
            });
            Evnts.Add(new Event() {
                name = "Æbleskiver dag",
                description = "This is a test",
                startDate = new DateTime(2015, 11, 3, 22, 00, 00),
                endDate = new DateTime(2015, 11, 3, 23, 00, 00)
            });
            Evnts.Add(new Event() {
                name = "Nedren dag",
                description = "This is a test",
                startDate = new DateTime(2016, 10, 3, 22, 00, 00),
                endDate = new DateTime(2016, 10, 6, 23, 00, 00)
            });
            /*Test data ends here*/

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

            EventList lowertlp = new EventList();

            foreach (var item in Evnts.OrderBy((Event e) => e.startDate)) {
                lowertlp.Controls.Add(new EventItem(item));
            }

            Button addEvent = new Button();
            addEvent.Height = 20;
            addEvent.Width = 100;
            addEvent.Text = "Add Event";
            //addEvent.Click += new System.EventHandler(this.);

            topflp.Controls.Add(addEvent);           
            headtlp.Controls.Add(topflp);
            headtlp.Controls.Add(lowertlp);

            Controls.Add(headtlp);
            
        }

        public override void Save() {
            throw new NotImplementedException();
        }
    }
}
