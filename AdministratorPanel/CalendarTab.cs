using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Xml.Serialization;
using System.IO;

namespace AdministratorPanel {
    public class CalendarTab : AdminTabPage {
        private Calendar calendar;
        private List<Room> rooms;
        public List<CalendarDay> calDay = new List<CalendarDay>();

        public CalendarTab() {
            Load();
            //Random rand = new Random(100);
            //CalendarDay test = new CalendarDay() { fullness = 1, isFullChecked = false, reservations = new List<Reservation>(), theDay = DateTime.Now.Date };
            
            //for (int i = 0; i < 3; i++)
            //{
            //    Reservation res = new Reservation { time = DateTime.Now.Date, name = "fish", numPeople = rand.Next(1, 100), pending = true };
            //    test.reservations.Add(res);
            //}
            //calDay.Add(test);

            Text = "Calendar";

            TableLayoutPanel outerTable = new TableLayoutPanel();
            outerTable.Dock = DockStyle.Fill;
            outerTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            outerTable.RowCount = 1;
            outerTable.ColumnCount = 2;
            Controls.Add(outerTable);

            //// Left Side
            TableLayoutPanel leftTable = new TableLayoutPanel();
            leftTable.Dock = DockStyle.Left;
            leftTable.AutoSize = true;
            leftTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //leftTable.BorderStyle = BorderStyle.Fixed3D;
            outerTable.Controls.Add(leftTable);

            calendar = new Calendar();
            //calendar.Dock = DockStyle.Top;
            //calendar.Anchor = AnchorStyles.Top;
            leftTable.Controls.Add(calendar);

            PendingReservationList b = new PendingReservationList(calendar, this);
            leftTable.Controls.Add(b);

            //// Right side
            //ReservationList rightTable = new ReservationList(calendar, reservations); TODO: fix
            //rightTable.makeItems(reservations, DateTime.Today);
            //outerTable.Controls.Add(rightTable); TODO: fix

        }

        public override void Save() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CalendarDay>));
            using (StreamWriter textWriter = new StreamWriter(@"Reservations.xml")) {
                serializer.Serialize(textWriter, calDay);
            }
        }

        public override void Load() {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CalendarDay>));
            using (FileStream fileReader = new FileStream(@"Reservations.xml", FileMode.OpenOrCreate)) {
                try {
                    calDay = deserializer.Deserialize(fileReader) as List<CalendarDay>;
                }
                catch (Exception) { }
            }
        }
    }
}
