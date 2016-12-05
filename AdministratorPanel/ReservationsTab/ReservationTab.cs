using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Shared;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Linq;

namespace AdministratorPanel {
    public class ReservationTab : AdminTabPage {
        private Calendar calendar;

        public ReservationList reservationList;
        public PendingReservationList pendingReservationList;
        public int reserveSpaceValue;
        private ReservationController reservationController;
        public bool lockedRes = false;
        private FormProgressBar probar;
        public CheckBox lockResevations = new CheckBox() {
            Text = "Lock Reservations",
        };

        ToolTip tooltip = new ToolTip() {
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
            Dock = DockStyle.Left,
            Minimum = 0,
            Step = 1,
            Width = 200
        };

        public ProgressBar reserveSpaceWithoutPending = new ProgressBar() {
            Style = ProgressBarStyle.Continuous,
            Dock = DockStyle.Left,
            Minimum = 0,
            Step = 1,
            Width = 200
        };

        Button addReservation = new Button() {
            Height = 20,
            Width = 100,
            Dock = DockStyle.Right,
            Text = "Add Reservation",

        };

        TableLayoutPanel outerTable = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize,
            RowCount = 1,
            ColumnCount = 2,

        };

        TableLayoutPanel progressbars = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            Height = 60,
            RowCount = 2,
            ColumnCount = 1,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };

        // Left Side
        TableLayoutPanel leftTable = new TableLayoutPanel() {
            Dock = DockStyle.Left,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,

        };

        TableLayoutPanel rightTable = new TableLayoutPanel() {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
        };

        TableLayoutPanel topRightTable = new TableLayoutPanel() {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 6,
        };

        Button roomsButton = new Button() {
            Text = "Modify Rooms",
            AutoSize = true,

        };
        Button testButton = new Button() {
            Text = "TEST Add res",
            AutoSize = true
        };

        public ReservationTab(ReservationController reservationController, FormProgressBar probar) {

            this.reservationController = reservationController;
            this.probar = probar;
            
            //TODO: temorary debug
            reservationController.rooms.Clear();
            reservationController.addRoom(new Room() { name = "Testroom", seats = 100 });
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
            

            Text = "Reservations";

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
            testButtonfunc();
            progressbars.Controls.Add(reserveSpaceWithPending);
            progressbars.Controls.Add(reserveSpaceWithoutPending);
            topRightTable.Controls.Add(lockResevations);
            topRightTable.Controls.Add(autoAcceptPresentage);
            topRightTable.Controls.Add(maxAutoAccept);
            topRightTable.Controls.Add(addReservation);


            probar.addToProbar();                               //For progress bar. 7


            subscriberList();
            probar.addToProbar();                               //For progress bar. 8
            tooltipController();
            probar.addToProbar();                               //For progress bar. 9
            //Hack

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
                MessageBox.Show("Not implemented");
            };

            addReservation.Click += (s, e) => {
                ReservationPopupBox p = new ReservationPopupBox(reservationController);
                p.Show();
            };

            lockResevations.CheckedChanged += (s, e) => {
                
                CalendarDay day = reservationController.findDay(calendar.SelectionRange.Start);
                
                day.isLocked = lockResevations.Checked;

                if(reservationController.checkIfRemove(day)) 
                    reservationController.reservationsCalendar.Remove(day);

                updateCheck(day);

                updateProgressBar(day);

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
            
            int tempNr;
            string tempstr = autoAcceptPresentage.Text;
            if (tempstr == "0") {
                day.isAutoaccept = false;
                Console.WriteLine("auto accept is false");
                tempNr = 0;
            } else {
                day.isAutoaccept = true;

                try {
                    int.TryParse(tempstr, out tempNr);
                    if (tempNr > 100 || tempNr < 0)
                        throw new Exception();
                } catch (Exception) {
                    MessageBox.Show("Please input a valid integer, that is minimum 0 or maximum 100");
                    tempNr = day.acceptPresentage;
                    autoAcceptPresentage.Text = tempNr.ToString();
                }


            }
            day.acceptPresentage = tempNr;
            updateCheck(day);
        }

        private void maxAutoAcceptBox() {
            CalendarDay day = reservationController.findDay(calendar.SelectionRange.Start);
            

            int tempNr;
            string tempstr = maxAutoAccept.Text;
            if (tempstr == "0") {
                tempNr = 501;
            } else {
                day.isAutoaccept = true;

                try {
                    int.TryParse(tempstr, out tempNr);
                    if (tempNr > 100 || tempNr < 0)
                        throw new Exception();
                } catch (Exception) {
                    MessageBox.Show("Please input a valid integer. ");
                    tempNr = day.acceptPresentage;
                    autoAcceptPresentage.Text = tempNr.ToString();
                }
                    

            }
            day.autoAcceptMaxPeople = tempNr;
            updateCheck(day);
        }

        private void tooltipController() {
            tooltip.SetToolTip(autoAcceptPresentage, "Percent of room filled to stop auto accept" + Environment.NewLine + 
                                                     "0 for no auto accept, 100 for acceptance of every valid resevations untill filled.");
            tooltip.SetToolTip(lockResevations, "If checked all resevations will get auto declined, as no more resevations can occure for said day.");
            tooltip.SetToolTip(addReservation, "Manually add a resevation");
            tooltip.SetToolTip(maxAutoAccept, "What is the maximum number of people that the program should auto accept." + Environment.NewLine + "0 for allow all sizes.");
            
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

        public void updateProgressBar(CalendarDay day) {
            try {
                
                int reservedSpace = 0;
                int reservedSpaceWpending = 0;
                if (day != null) {
                    foreach (var item in day.reservations.Where(x => x.state == Reservation.State.Accepted))
                        reservedSpace += item.numPeople;
                    foreach (var item in day.reservations.Where(x => x.state != Reservation.State.Denied))
                        reservedSpaceWpending += item.numPeople;
                }
                reserveSpaceWithPending.Value = 0;
                reserveSpaceWithoutPending.Value = 0;
                reserveSpaceWithPending.Maximum = day?.numSeats ?? 1;
                reserveSpaceWithPending.Value = reservedSpaceWpending;
                
                reserveSpaceWithoutPending.Maximum = day?.numSeats ?? 1;
                reserveSpaceWithoutPending.Value = reservedSpace;

                tooltip.SetToolTip(reserveSpaceWithoutPending, "The status of seats left, not counting the not accepted resevations." + ((day != null)? Environment.NewLine +
                                         "Status: " + reserveSpaceWithoutPending.Value.ToString() + " out of " + reserveSpaceWithoutPending.Maximum.ToString() : ""));

                tooltip.SetToolTip(reserveSpaceWithPending, "The status of seats left, including the pending resevations." + ((day != null)? Environment.NewLine + 
                                         "Status: " + reserveSpaceWithPending.Value.ToString() + " out of " + reserveSpaceWithPending.Maximum.ToString() : ""));

            } catch (Exception e ) {
                MessageBox.Show(e.Message,"FATAL ERROR");
                // We don't care too much about this
                reserveSpaceWithPending.Maximum = 1;
                reserveSpaceWithPending.Value = 1;
                reserveSpaceWithoutPending.Maximum = 1;
                reserveSpaceWithoutPending.Value = 1;
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

            res.time = (rand.Next(0, 5) == 1) ? DateTime.Now : new DateTime(2016, 12, 2 /*rand.Next(1, 30)*/);
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
