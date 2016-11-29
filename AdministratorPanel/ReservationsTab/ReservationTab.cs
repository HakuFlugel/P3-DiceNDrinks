using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;

namespace AdministratorPanel
{
    public class ReservationTab : AdminTabPage
    {
        private Calendar calendar;

        public ReservationList reservationList;
        public PendingReservationList pendingReservationList;

        public ProgressBar reserveSpace = new ProgressBar();
        public int reserveSpaceValue;
        public Label reserveSpaceText = new Label();
        public CheckBox reservationFull = new CheckBox();
        private ReservationController reservationController;

        public ReservationTab(ReservationController reservationController)
        {

            this.reservationController = reservationController;

            //TODO: temorary debug
            reservationController.rooms.Clear();
            reservationController.addRoom(new Room() {name = "Testroom", seats = 100});

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

            pendingReservationList = new PendingReservationList(calendar, this, reservationController);
            leftTable.Controls.Add(pendingReservationList);


            //// Right side
            TableLayoutPanel rightTable = new TableLayoutPanel();
            rightTable.Dock = DockStyle.Fill;
            rightTable.AutoSize = true;
            rightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            outerTable.Controls.Add(rightTable);

            //// Right side
            TableLayoutPanel topRightTable = new TableLayoutPanel();
            topRightTable.Dock = DockStyle.Top;
            topRightTable.AutoSize = true;
            topRightTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            topRightTable.ColumnCount = 4;
            rightTable.Controls.Add(topRightTable);

            Button roomsButton = new Button();
            roomsButton.Text = "Modify Rooms";
            roomsButton.AutoSize = true;
            roomsButton.Click += (sender, args) =>
            {
                MessageBox.Show("Not implemented");
            };
            topRightTable.Controls.Add(roomsButton);

            reserveSpace.Style = ProgressBarStyle.Continuous;
            reserveSpace.Dock = DockStyle.Left;
            reserveSpace.Minimum = 0;
            reserveSpace.Step = 1;
            reserveSpace.Width = 200;

            reserveSpaceText.Dock = DockStyle.Left;
            reserveSpaceText.Font = new Font("Arial", 16);
            reservationFull.Text = "Lock Reservations";
//reservationFull.Checked

//          topRightTable.Controls.Add(reserveSpaceText);
            topRightTable.Controls.Add(reserveSpace);
            topRightTable.Controls.Add(reservationFull);

            Button addReservation = new Button();
            addReservation.Height = 20;
            addReservation.Width = 100;
            addReservation.Dock = DockStyle.Right;
            addReservation.Text = "Add Reservation";
            addReservation.Click += (s, e) =>
            {
                ReservationPopupBox p = new ReservationPopupBox(reservationController);
                p.Show();
            };
            topRightTable.Controls.Add(addReservation);

            reservationList = new ReservationList(calendar, reservationController); /*TODO: fix*/
            rightTable.Controls.Add(reservationList);
            reservationFull.Click += (s, e) => { reservationList.lockReservations(reservationFull.Checked); };
            //rightTable.updateCurrentDay(reservations, DateTime.Today);
            /*TODO: fix*/

            calendar.DateSelected += (sender, args) =>
            {
                //TODO: move some of it out of here to its own class
                CalendarDay day = reservationController.reservationsCalendar.Find(o => o.theDay.Date == args.Start.Date);
                reservationFull.Checked = day?.isFullChecked ?? false;
                updateProgressBar(day);

            };

            reservationController.ReservationUpdated += (sender, args) =>
            {
                CalendarDay day = reservationController.reservationsCalendar.Find(o => o.theDay.Date == calendar.SelectionStart.Date);

                updateProgressBar(day);
            };

            //TODO: hack
            updateProgressBar(reservationController.reservationsCalendar.Find(o => o.theDay.Date == DateTime.Today));
        }

        public void updateProgressBar(CalendarDay day)
        {
            try
            {
                day?.calculateSeats(reservationController); // TODO: we are not doing this elsewere right now
                reserveSpace.Maximum = day?.numSeats ?? 1;
                reserveSpace.Value = day?.reservedSeats ?? 0;
            }
            catch (Exception)
            {
                // We don't care too much about this
                reserveSpace.Maximum = 1;
                reserveSpace.Value = 1;
            }
        }

        // TODO: functions...
        public override void Save() {
//            XmlSerializer serializer = new XmlSerializer(typeof(List<CalendarDay>));
//            using (StreamWriter textWriter = new StreamWriter(@"Reservations.xml")) {
//                serializer.Serialize(textWriter, calDayList);
//            }
        }

        public override void Load() {
//            XmlSerializer deserializer = new XmlSerializer(typeof(List<CalendarDay>));
//            using (FileStream fileReader = new FileStream(@"Reservations.xml", FileMode.OpenOrCreate)) {
//                try {
//                    calDayList = deserializer.Deserialize(fileReader) as List<CalendarDay>;
//                }
//                catch (Exception) { }
//            }
        }
    }
}
