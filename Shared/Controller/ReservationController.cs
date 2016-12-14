using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

// TODO: reservation af lokale osv. : Vi har List<int> med de rum der har her. Har List<int> på hver dag, hvor de gennem en checkbox eller lignende kan tilføje/fjerne en sådan reservation

namespace Shared
{
    public class ReservationController : ControllerBase
    {

        public int totalSeats;
        public List<Room> rooms = new List<Room>();
        public List<CalendarDay> reservationsCalendar = new List<CalendarDay>();

        public event EventHandler ReservationUpdated;

        //TODO: make sure it is pending if from user

        public void addReservation(Reservation reservation) {

            addToDay(reservation);
            //ReservationAdded?.Invoke(this, new AddReservationEventArgs(reservation));
            ReservationUpdated?.Invoke(this, EventArgs.Empty);

            CalendarDay tempday = findDay(reservation.time);

            if (tempday.isLocked)
                reservation.state = Reservation.State.Denied;
            else
                checkIfAutoAccept(reservation, tempday);

            save();
        }
        //TODO: make sure it is pending if from user
        public void updateReservation(Reservation reservation)
        {
            Reservation oldReservation =
                reservationsCalendar.SelectMany(cd => cd.reservations).First(r => r.id == reservation.id);

            reservation.created = oldReservation.created;

            removeFromDay(oldReservation);

            addToDay(reservation);
            //if(reservation.time.Date != oldReservation.time.Date)
            //    checkIfAutoAccept(reservation, findDay(reservation.time));

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
            //resDay.reservedSeats += reservation.numPeople;

        }

        public CalendarDay findDay(DateTime date) {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay.Date == date.Date);
            if (resDay == null) {
                resDay = new CalendarDay() { theDay = date.Date };
                reservationsCalendar.Add(resDay);
            }
            resDay.calculateSeats(this); //TODO: maybe it can be moved into the if above

            return resDay;
        }
        public bool checkIfRemove(CalendarDay day) {
            if (day.theDay < DateTime.Today.AddDays(-1) || !day.isLocked && day.reservations.Count < 1
                 && day.defaultAcceptMaxPeople == day.autoAcceptMaxPeople
                 && day.defaultAcceptPresentage == day.acceptPresentage)

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
        }
        public void checkIfAutoAccept(Reservation reservation, CalendarDay resDay) {


            //Console.WriteLine(resDay == null? "DAY IS NULL" : resDay.isAutoaccept.ToString() + " " + resDay.acceptPresentage.ToString() + " <= " + "(" + resDay.reservedSeats.ToString() + "+" + reservation.numPeople.ToString() + ")*100 / " + resDay.numSeats.ToString());


            if(resDay != null ) {
                if (resDay.numSeats == 0)
                    resDay.calculateSeats(this);
                    resDay.calculateReservedSeats();
//                foreach (var item in resDay.reservations.Where(x => x.state == Reservation.State.Accepted))
//                    reservedSeats += item.numPeople;
            }

            if ((resDay == null && reservation.numPeople <= 5
             || (!resDay.isLocked
             && resDay.autoAcceptMaxPeople >= reservation.numPeople
             && resDay.isAutoaccept //maybe
             && resDay.acceptPresentage >= (resDay.reservedSeats + reservation.numPeople) * 100 / resDay.numSeats))
             && reservation.state != Reservation.State.Accepted && !reservation.forcedByAdmin) {

                reservation.state = Reservation.State.Accepted;
            } else
                return;
            updateReservation(reservation);

        }


        public override void load()
        {
            reservationsCalendar = loadFile<CalendarDay>("reservationsCalendar");
            rooms = loadFile<Room>("rooms");

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
    }
}