using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

namespace AdministratorPanel {
    public class CalendarTab : AdminTabPage {
        private Calendar calendar;
        private List<Room> rooms;
        public List<CalendarDay> calDayList = new List<CalendarDay>();

        public ReservationList reservationList;
        public PendingReservationList pendingReservationList;

        public ProgressBar reserveSpace = new ProgressBar();
        public int reserveSpaceValue;
        public Label reserveSpaceText = new Label();
        public CheckBox reservationFull = new CheckBox();

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

            pendingReservationList = new PendingReservationList(calendar, this);
            leftTable.Controls.Add(pendingReservationList);


            //// Right side
            TableLayoutPanel rightTable = new TableLayoutPanel();
            rightTable.Dock = DockStyle.Fill;
            rightTable.AutoSize = true;
            rightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            outerTable.Controls.Add(rightTable);

            //// Right side
            TableLayoutPanel topRightTable = new TableLayoutPanel();
            topRightTable.Dock = DockStyle.Fill;
            topRightTable.AutoSize = true;
            topRightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            topRightTable.ColumnCount = 4;
            rightTable.Controls.Add(topRightTable);

            reserveSpace.Style = ProgressBarStyle.Continuous;
            reserveSpace.Dock = DockStyle.Left;
            reserveSpace.Minimum = 0;
            reserveSpaceText.Dock = DockStyle.Left;
            reserveSpaceText.Font = new Font("Arial", 16);
            reservationFull.Text = "Lock Reservations";

            topRightTable.Controls.Add(reserveSpaceText);
            topRightTable.Controls.Add(reserveSpace);
            topRightTable.Controls.Add(reservationFull);
            
            Button addReservation = new Button();
            addReservation.Height = 20;
            addReservation.Width = 100;
            addReservation.Dock = DockStyle.Right;
            addReservation.Text = "Add Reservation";
            addReservation.Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(this);
                p.Show();
            };
            topRightTable.Controls.Add(addReservation);

            reservationList = new ReservationList(calendar, this); /*TODO: fix*/
            rightTable.Controls.Add(reservationList);
            reservationFull.Click += reservationList.lockReservations;
            //rightTable.makeItems(reservations, DateTime.Today);
            /*TODO: fix*/


        }

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
