using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Shared
{
    public class ReservationController : ControllerBase
    {

        public int totalSeats;
        public List<Room> rooms = new List<Room>();
        public List<CalendarDay> reservationsCalendar = new List<CalendarDay>();

        public class AutoAcceptSettings
        {
            public int defaultAcceptPercentage = 50;
            public int defaultAcceptMaxPeople = 5;
        }
        public AutoAcceptSettings autoAcceptSettings = new AutoAcceptSettings();


        public event EventHandler ReservationUpdated;


        public void addReservation(Reservation reservation) {

            addToDay(reservation);
            //ReservationAdded?.Invoke(this, new AddReservationEventArgs(reservation));
            ReservationUpdated?.Invoke(this, EventArgs.Empty);

            CalendarDay tempday = findDay(reservation.time);

            if(reservation.state == Reservation.State.Pending)
                checkIfAutoAccept(reservation, findDay(reservation.time));

            save();
        }
        public void updateReservation(Reservation reservation)
        {
            Reservation oldReservation =
                reservationsCalendar.SelectMany(cd => cd.reservations).First(r => r.id == reservation.id);

            reservation.created = oldReservation.created;

            removeFromDay(oldReservation);

            addToDay(reservation);
            if(reservation.state == Reservation.State.Pending)
                checkIfAutoAccept(reservation, findDay(reservation.time));

            ReservationUpdated?.Invoke(this, EventArgs.Empty);

            save();
        }

        public void removeReservation(Reservation reservation) {

            removeFromDay(reservation);

            //ReservationRemoved?.Invoke(this, new RemoveReservationEventArgs(reservation));
            ReservationUpdated?.Invoke(this, EventArgs.Empty);

            save();
        }

        private Random rand = new Random();

        public ReservationController(string path = "data/") : base(path)
        {
        }

        public int getRandomID()
        {
            int id;

            do id = rand.Next(); while (reservationsCalendar.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        private void addToDay(Reservation reservation) {

            CalendarDay resDay = findDay(reservation.time);

            resDay.reservations.Add(reservation);
            resDay.calculateReservedSeats();

        }

        public CalendarDay findDay(DateTime date) {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay.Date == date.Date);
            if (resDay == null) {
                resDay = new CalendarDay(autoAcceptSettings.defaultAcceptPercentage, autoAcceptSettings.defaultAcceptMaxPeople) { theDay = date.Date };
                reservationsCalendar.Add(resDay);
            }
            resDay.calculateSeats(this);

            return resDay;
        }
        public bool checkIfRemove(CalendarDay day) {
            if (day.theDay < DateTime.Today.AddDays(-1) || !day.isLocked && day.reservations.Count < 1
                 && autoAcceptSettings.defaultAcceptMaxPeople == day.autoAcceptMaxPeople
                 && autoAcceptSettings.defaultAcceptPercentage == day.acceptPercentage)

                return true;
            else
                return false;
        }

        private void removeFromDay(Reservation reservation) {
            CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);

            resDay.reservations.RemoveAll(r => r.id == reservation.id);
            //resDay.reservations.Remove(reservation);
            resDay.calculateReservedSeats();

            if(checkIfRemove(resDay))
                reservationsCalendar.Remove(resDay);

            //resDay.reservedSeats -= reservation.numPeople;
        }

        public void submitRooms(List<Room> rooms)
        {
            List<Room> toremove = new List<Room>();
            foreach (var existingroom in this.rooms)
            {
                if (!rooms.Any(r => r.name == existingroom.name))
                {
                    toremove.Add(existingroom);
                    //reservationController.removeRoom(existingroom);
                }
            }
            foreach (var room in toremove)
            {
                removeRoom(room);
            }


            foreach (var room in rooms)
            {

                if (this.rooms.Any(r => r.name == room.name))
                {
                    changeRoom(room);
                }
                else
                {
                    addRoom(room);
                }
            }

            save();

            ReservationUpdated?.Invoke(this, EventArgs.Empty);

        }

        private void addRoom(Room room) {
            rooms.Add(room);
            calculateTotalSeats();

            foreach (var day in reservationsCalendar)
            {
                day.calculateSeats(this);
            }

            ReservationUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void removeRoom(Room room)
        {
            rooms.Remove(room);
            calculateTotalSeats();

            foreach (var day in reservationsCalendar) {
                day.unreserveRoom(this, room);
                //day.roomsReserved.Remove(room);
                //day.calculateSeats(this);
            }

            ReservationUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void changeRoom(Room room)
        {

            Room oldroom = rooms.First(r => r.name == room.name);

            rooms[rooms.IndexOf(oldroom)] = room;
            calculateTotalSeats();

            foreach (var day in reservationsCalendar) {
                int roomindex = day.roomsReserved.IndexOf(oldroom);
                if (roomindex < 0) continue;

                day.roomsReserved[roomindex] = room;
                day.calculateSeats(this);
            }

            ReservationUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void calculateTotalSeats()
        {
            totalSeats = rooms.Sum(r => r.seats);
        }

        public override void save()
        {
            saveFile("reservationsCalendar", reservationsCalendar);
            saveFile("rooms", rooms);
            saveAutoAccept();
        }
        public void checkIfAutoAccept(Reservation reservation, CalendarDay resDay) {


            //Console.WriteLine(resDay == null? "DAY IS NULL" : resDay.isAutoaccept.ToString() + " " + resDay.acceptPercentage.ToString() + " <= " + "(" + resDay.reservedSeats.ToString() + "+" + reservation.numPeople.ToString() + ")*100 / " + resDay.numSeats.ToString());


            if (resDay.numSeats == 0)
                resDay.calculateSeats(this);
                resDay.calculateReservedSeats();

            if (resDay.isLocked)
            {
                reservation.state = Reservation.State.Denied;
            }
            else if (reservation.numPeople <= resDay.autoAcceptMaxPeople
                    && reservation.numPeople <= resDay.numSeats
                    && resDay.isAutoaccept
                    && (resDay.reservedSeats + reservation.numPeople) * 100 / resDay.numSeats <= resDay.acceptPercentage)
            {
                reservation.state = Reservation.State.Accepted;
            }


            ReservationUpdated?.Invoke(this, EventArgs.Empty);

        }


        public override void load()
        {
            reservationsCalendar = loadFile<CalendarDay>("reservationsCalendar");
            rooms = loadFile<Room>("rooms");
            loadAutoAccept();

            calculateTotalSeats();
        }

        public void submitReservedRooms(List<Room> list, DateTime day)
        {

            CalendarDay calendarDay = findDay(day);

            calendarDay.roomsReserved = list;
            calculateTotalSeats();
            calendarDay.calculateSeats(this);

            save();

            ReservationUpdated?.Invoke(this, EventArgs.Empty);
        }


        protected void saveAutoAccept()
        {
            Directory.CreateDirectory(path);

            using (StreamWriter streamWriter = new StreamWriter(path + "autoaccept" + ext))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, autoAcceptSettings);
            }

        }

        protected void loadAutoAccept()
        {

            Directory.CreateDirectory(path);
            try
            {
                using (StreamReader streamReader = new StreamReader(path + "autoaccept" + ext))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    autoAcceptSettings = jsonSerializer.Deserialize<AutoAcceptSettings>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("autoaccept not found");
            }

            if (autoAcceptSettings == null)
            {
                Console.WriteLine("autoaccept was null after loading... setting it to new list");
                autoAcceptSettings = new AutoAcceptSettings();
            }

        }
    }
}