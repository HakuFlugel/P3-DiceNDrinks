using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Xml.Serialization;
using System.IO;

namespace AdministratorPanel {
    public class ReservationTab : AdminTabPage {
        private Calendar calendar;

        public ReservationList reserveationList;
        public PendingReservationList pendingReservationList;

        public ReservationTab() {
            Load();
            //Random rand = new Random(100);
            //CalendarDay test = new CalendarDay() { fullness = 1, isFullChecked = false, reservations = new List<Reservation>(), theDay = DateTime.Now.Date };
            
            //for (int i = 0; i < 3; i++)
            //{
            //    Reservation res = new Reservation { time = DateTime.Now.Date, name = "fish", numPeople = rand.Next(1, 100), pending = true };
            //    test.reservations.Add(res);
            //}
            //calDay.Add(test);

            Text = "Reservations";

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

            pendingReservationList = new PendingReservationList(calendar, this);
            leftTable.Controls.Add(pendingReservationList);


            //// Right side
            TableLayoutPanel rightTable = new TableLayoutPanel();
            rightTable.Dock = DockStyle.Fill;
            rightTable.AutoSize = true;
            rightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            outerTable.Controls.Add(rightTable);

            Button addReservation = new Button();
            addReservation.Height = 20;
            addReservation.Width = 100;
            addReservation.Dock = DockStyle.Right;
            addReservation.Text = "Add Reservation";
            addReservation.Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(this);
                p.Show();
            };
            rightTable.Controls.Add(addReservation);

            reserveationList = new ReservationList(calendar, this); /*TODO: fix*/
            rightTable.Controls.Add(reserveationList);
            //rightTable.makeItems(reservations, DateTime.Today);
             /*TODO: fix*/
            

        }


        // TODO: functions...
        public override void Save() {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CalendarDay>));
            using (StreamWriter textWriter = new StreamWriter(@"Reservations.xml")) {
                serializer.Serialize(textWriter, calDayList);
            }
        }

        public override void Load() {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CalendarDay>));
            using (FileStream fileReader = new FileStream(@"Reservations.xml", FileMode.OpenOrCreate)) {
                try {
                    calDayList = deserializer.Deserialize(fileReader) as List<CalendarDay>;
                }
                catch (Exception) { }
            }
        }
    }
}
