using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Drawing;
using System.Linq;

namespace AdministratorPanel {
    public class ReservationTab : AdminTabPage {
        public Calendar calendar;

        public ReservationList reservationList;
        public PendingReservationList pendingReservationList;
        public int reserveSpaceValue;
        private ReservationController reservationController;
        public bool lockedRes = false;
        private FormProgressBar probar;

        public CheckBox lockResevations = new CheckBox() {
            Text = "Lock Reservations",
        };

        private ToolTip tooltip = new ToolTip() {
            AutoPopDelay = 5000,
            InitialDelay = 100,
            ReshowDelay = 500,
            ShowAlways = true
        };

        public NiceTextBox autoAcceptPresentage = new NiceTextBox() {
            Width = 30,
            MaxLength = 3,
        };

        public NiceTextBox maxAutoAccept = new NiceTextBox() {
            Width = 30,
            MaxLength = 2,
        };

        public Label reserveSpaceText = new Label() {
            Dock = DockStyle.Left,
            Font = new Font("Arial", 16),
        };

        public ProgressBar reserveSpaceWithPending = new ProgressBar() {
            Style = ProgressBarStyle.Continuous,
            //Dock = DockStyle.Left,
            Minimum = 0,
            Step = 1,
            Width = 192,
            Height = 12,
            Margin = Padding.Empty
        };

        public ProgressBar reserveSpaceWithoutPending = new ProgressBar() {
            Style = ProgressBarStyle.Continuous,
            //Dock = DockStyle.Left,
            Minimum = 0,
            Step = 1,
            Width = 192,
            Height = 12,
            Margin = Padding.Empty
        };

        private Button addReservation = new Button() {
            Height = 20,
            Width = 100,
            Dock = DockStyle.Right,
            Text = "Add Reservation",

        };

        private TableLayoutPanel outerTable = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
            RowCount = 1,
            ColumnCount = 2,

        };

        private TableLayoutPanel progressbars = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            Height = 16,
            RowCount = 2,
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        private Label remainingSeats = new Label()
        {
            Text = "0",
            Font = new Font(DefaultFont.FontFamily, 16),
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true,
            //TODO:tooltipbelow
        };

        // Left Side
        private TableLayoutPanel leftTable = new TableLayoutPanel() {
            Dock = DockStyle.Left,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,

        };

        private TableLayoutPanel rightTable = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
        };

        private TableLayoutPanel topRightTable = new TableLayoutPanel() {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 7,
        };

        private Button roomsButton = new Button() {
            Text = "Modify Rooms",
            AutoSize = true,

        };
        private Button testButton = new Button() {
            Text = "TEST Add res",
            AutoSize = true
        };

        public ReservationTab(ReservationController reservationController, FormProgressBar probar) {
            //tab name
            Text = "Reservations";

            this.reservationController = reservationController;
            this.probar = probar;
            
            //TODO: temorary debug
            //reservationController.rooms.Clear();
            //reservationController.addRoom(new Room() { name = "Testroom", seats = 100 });
            List<CalendarDay> toremove = new List<CalendarDay>();
            probar.addToProbar();                               //For progress bar. 1

            foreach (var item in reservationController.reservationsCalendar)
                if (reservationController.checkIfRemove(item))
                    toremove.Add(item);

            foreach (var item in toremove)
                reservationController.reservationsCalendar.Remove(item);

            probar.addToProbar();                               //For progress bar. 2

            CalendarDay tempDate = (reservationController.reservationsCalendar.Find(x => x.theDay == DateTime.Today));
            autoAcceptPresentage.Text = (tempDate != null) ? tempDate.acceptPresentage.ToString() : "50" ;
            maxAutoAccept.Text = (tempDate != null) ? tempDate.autoAcceptMaxPeople.ToString() : "5";

            Controls.Add(outerTable);

            probar.addToProbar();                               //For progress bar. 3
            calendar = new Calendar();

            pendingReservationList = new PendingReservationList(calendar, this, reservationController);
            probar.addToProbar();                               //For progress bar. 4
            reservationList = new ReservationList(calendar, reservationController); /*TODO: fix*/
            probar.addToProbar();                               //For progress bar. 5

            // Left side
            outerTable.Controls.Add(leftTable);
            leftTable.Controls.Add(calendar);
            leftTable.Controls.Add(pendingReservationList);

            probar.addToProbar();                               //For progress bar. 6
            // Right side
            outerTable.Controls.Add(rightTable);
            rightTable.Controls.Add(topRightTable);
            rightTable.Controls.Add(reservationList);
            topRightTable.Controls.Add(roomsButton);
            topRightTable.Controls.Add(progressbars);
            progressbars.Controls.Add(reserveSpaceWithPending);
            progressbars.Controls.Add(reserveSpaceWithoutPending);

            topRightTable.Controls.Add(remainingSeats);

            topRightTable.Controls.Add(lockResevations);
            topRightTable.Controls.Add(autoAcceptPresentage);
            topRightTable.Controls.Add(maxAutoAccept);
            topRightTable.Controls.Add(addReservation);


            probar.addToProbar();                               //For progress bar. 7

            subscriberList();
            probar.addToProbar();                               //For progress bar. 8
            tooltipController();
            probar.addToProbar();                               //For progress bar. 9

            updateProgressBar(reservationController.reservationsCalendar.Find(o => o.theDay.Date == DateTime.Today));
            probar.addToProbar();                               //For progress bar. 10
        }

        private void subscriberList() {

            reservationController.ReservationUpdated += (sender, args) => {
                CalendarDay day = reservationController.reservationsCalendar.Find(o => o.theDay.Date == calendar.SelectionStart.Date);

                updateProgressBar(day);
            };

            calendar.DateChanged += (s, e) => {
                CalendarDay day = reservationController.reservationsCalendar.Find(o => o.theDay.Date == e.Start.Date);

                lockResevations.Checked = (day != null) ? (day.isFullChecked || day.isLocked) : false;

                updateProgressBar(day);

                autoAcceptPresentage.Text = (day != null) ? day.acceptPresentage.ToString() : "50";

                maxAutoAccept.Text = (day != null) ? ((day.autoAcceptMaxPeople == 501) ? "0" : day.autoAcceptMaxPeople.ToString()) : "5";
                    
            };
            
            autoAcceptPresentage.LostFocus += (s, e) => {
                autoAcceptBox();
            };

            autoAcceptPresentage.KeyPress += (s, e) => {
                if (e.KeyChar != (char)Keys.Enter)
                    return;
                autoAcceptBox();
                
            };

            roomsButton.Click += (sender, args) => {

                RoomPopup rp = new RoomPopup(reservationController);
                rp.Show();
            };

            addReservation.Click += (s, e) => {

                ReservationPopupBox p = new ReservationPopupBox(reservationController);
            };

            lockResevations.CheckedChanged += (s, e) => {
                CheckedChanged();
            };

            maxAutoAccept.LostFocus += (s, e) => {
                maxAutoAcceptBox();
            };

            maxAutoAccept.KeyPress += (s, e) => {
                if (e.KeyChar != (char)Keys.Enter)
                    return;
                maxAutoAcceptBox();
            };
        }

        private void autoAcceptBox() {
            CalendarDay day = reservationController.findDay(calendar.SelectionRange.Start);

            if (autoAcceptPresentage.Text == day.acceptPresentage.ToString()) {
                return;
            }


            int tempNr;
            string tempstr = autoAcceptPresentage.Text;

            if (tempstr == "0") {
                day.isAutoaccept = false;
                Console.WriteLine("auto accept is false");
                tempNr = 0;
            } else {
                day.isAutoaccept = true;

                try {
                    tempNr = Int32.Parse(tempstr);
                    if (tempNr > 100 || tempNr < 0)
                        throw new FormatException();
                } catch (FormatException) {
                    autoAcceptPresentage.Text = day.acceptPresentage.ToString();
                    return;
                }
            }
            day.acceptPresentage = tempNr;
            updateCheck(day);
        }

        private void maxAutoAcceptBox() {
            CalendarDay day = reservationController.findDay(calendar.SelectionRange.Start);
            if (maxAutoAccept.Text == day.autoAcceptMaxPeople.ToString()) {

                return;
            }

            int tempNr;
            string tempstr = maxAutoAccept.Text;

            if (tempstr == "0") {
                tempNr = 501;
            } else {
                day.isAutoaccept = true;

                try {
                    tempNr = Int32.Parse(tempstr);
                    if (tempNr > 100 || tempNr < 0 )
                        throw new FormatException();
                } catch (FormatException) {
                    maxAutoAccept.Text = day.autoAcceptMaxPeople.ToString();
                    return;
                }
            }
            day.autoAcceptMaxPeople = tempNr;
            updateCheck(day);
        }

        public void CheckedChanged() {
            CalendarDay day = reservationController.findDay(calendar.SelectionRange.Start);

            day.isLocked = lockResevations.Checked;

            if (reservationController.checkIfRemove(day))
                reservationController.reservationsCalendar.Remove(day);

            updateCheck(day);

            updateProgressBar(day);
        }

        private void tooltipController() {
            tooltip.SetToolTip(autoAcceptPresentage, "Percent of available seats that will be autoaccepted" + Environment.NewLine +
                                                     "0 to disable auto accept, 100 to accept until all seats are filled.");
            tooltip.SetToolTip(lockResevations, "If checked all pending reservations will be declined, and no more reservations can be made");
            //tooltip.SetToolTip(addReservation, "Manually add a resevation");
            tooltip.SetToolTip(maxAutoAccept, "The largest reservation that should be auto accepted" + Environment.NewLine + "0 to allow all sizes.");
        }

        private void updateCheck(CalendarDay day) {
            List<Reservation> temp = new List<Reservation>();

            if (day.reservations.Count > 0) {

                foreach (var item in day.reservations) 
                    temp.Add(item);

                foreach(var item in temp) 
                    reservationController.checkIfAutoAccept(item, day);
            }
        }

        public void updateProgressBar(CalendarDay day)
        {

            int totalSeats = day?.numSeats ?? reservationController.totalSeats;
            int reservedSeats = day?.reservedSeats ?? 0;
            int pendingSeats = day?.reservedSeatsPending ?? 0;

            // Prevent exception from Value>Maximum
            reserveSpaceWithPending.Value = 0;
            reserveSpaceWithoutPending.Value = 0;

            reserveSpaceWithPending.Maximum = totalSeats;
            reserveSpaceWithPending.Value = Math.Min(reserveSpaceWithPending.Maximum, reservedSeats + pendingSeats);

            reserveSpaceWithoutPending.Maximum = totalSeats;
            reserveSpaceWithoutPending.Value = Math.Min(reserveSpaceWithoutPending.Maximum, day?.reservedSeats ?? 0);

            tooltip.SetToolTip(reserveSpaceWithoutPending, "Fullness counting only accepted reservations." + ((day != null)? Environment.NewLine +
                                     $"{reserveSpaceWithoutPending.Value} / {reserveSpaceWithoutPending.Maximum}" : ""));

            tooltip.SetToolTip(reserveSpaceWithPending, "Fulness, including the pending resevations." + ((day != null)? Environment.NewLine +
                                     $"{reserveSpaceWithPending.Value} / {reserveSpaceWithPending.Maximum}" : ""));

            remainingSeats.Text = $"{totalSeats - reservedSeats}";
            tooltip.SetToolTip(remainingSeats, $"Remaining seats {totalSeats - reservedSeats} / {totalSeats}");
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

        private void testButtonfunc() {
            
            topRightTable.Controls.Add(testButton);
            testButton.Click += (s, e) => {
                createResevation();
            };
        }

        public void createResevation() {
            string[] firstnames = {
                "Candyce","Leigh",
                "Carl","Klara","Kristan",
                "Deidre","Everette","Adelle",
                "Hulda","Dorthey","Shery",
                "Alfredia","Suzan","Marna","Kareem",
                "Tina","Kyong","Sherice",
                "Damian","Arnold" };

            string[] lastnames = {
                "Holtkamp","Lamirande","Nestor","Ferree","Donahue",
                "Montville","Neumeister","Hubert","Richarson","Mancino",
                "Padilla","Ehret","Claxton","Keyes","Staff","Tower",
                "Backstrom","Oglesby","Stanger","Flansburg"
            };

            string[] emailDomain = {
                "hotmail.com","hotmail.dk","gmail.com","mail.com","mail.dk",
                "hidemyass.com","webmail.com","webmail.dk","email.com",
                "email.dk","computer.dk","jordkanin.dk","hem.dk"
            };

            Random rand = new Random();
            Reservation res = new Reservation();

            string fnam = firstnames[rand.Next(0, firstnames.Count())];
            string lnam = lastnames[rand.Next(0, lastnames.Count())];
            res.name = fnam + " " + lnam;
            res.email = fnam + rand.Next(0, 425).ToString() + "@" + emailDomain[rand.Next(0,emailDomain.Count())];

            res.time = calendar.SelectionStart;//(rand.Next(0, 5) == 1) ? DateTime.Now : new DateTime(2016, 12, 2 /*rand.Next(1, 30)*/);
            res.state = Reservation.State.Pending;
            res.phone = rand.Next(0, 9).ToString() + rand.Next(0, 9).ToString() + rand.Next(0, 9).ToString() + 
                        rand.Next(0, 9).ToString() + rand.Next(0, 9).ToString() + rand.Next(0, 9).ToString() + 
                        rand.Next(0, 9).ToString() + rand.Next(0, 9).ToString();

            res.numPeople = rand.Next(1, 10);
            res.created = DateTime.Now;
            reservationController.addReservation(res);
            Console.WriteLine("Resevation added at: " + res.time.ToString());
        }
    }
}
