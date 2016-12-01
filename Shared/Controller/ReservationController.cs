using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft;

// TODO: reservation af lokale osv. : Vi har List<int> med de rum der har her. Har List<int> på hver dag, hvor de gennem en checkbox eller lignende kan tilføje/fjerne en sådan reservation

namespace Shared
{
    public class ReservationController : ControllerBase
    {

        public List<Room> rooms = new List<Room>();
        public List<CalendarDay> reservationsCalendar = new List<CalendarDay>();

/*        public event EventHandler<AddReservationEventArgs> ReservationAdded;

        public class AddReservationEventArgs
        {
            public Reservation newReservation;

            public AddReservationEventArgs(Reservation newReservation)
            {
                this.newReservation = newReservation;
            }
        }*/

        public event EventHandler<UpdateReservationEventArgs> ReservationUpdated;

        public class UpdateReservationEventArgs
        {
            public Reservation newReservation;
            public Reservation oldReservation;

            public UpdateReservationEventArgs(Reservation oldReservation, Reservation newReservation)
            {
                this.oldReservation = oldReservation;
                this.newReservation = newReservation;
            }
        }

/*        public event EventHandler<RemoveReservationEventArgs> ReservationRemoved;
        public class RemoveReservationEventArgs
        {
            public Reservation newReservation;

            public RemoveReservationEventArgs(Reservation newReservation)
            {
                this.newReservation = newReservation;
            }
        }*/

        public void addReservation(Reservation reservation)
        {
            reservation.id = getRandomID();
            reservation.created = DateTime.Now;

            addToDay(reservation);

            //ReservationAdded?.Invoke(this, new AddReservationEventArgs(newReservation));
            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(null, reservation));

        }

        public void updateReservation(Reservation oldReservation, Reservation newReservation)
        {
            bool hasMoved = newReservation.time.Date != oldReservation.time.Date;
            //TODO: do we need this if the if-else is commented out?


            newReservation.created = oldReservation.created;

            removeFromDay(oldReservation);
            addToDay(newReservation);

            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(oldReservation, newReservation));

        }

        public void removeReservation(Reservation reservation)
        {

            removeFromDay(reservation);

            //ReservationRemoved?.Invoke(this, new RemoveReservationEventArgs(newReservation));
            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(reservation, null));
        }

        private Random rand = new Random();

        public ReservationController(string path = "data/") : base(path)
        {
        }

        private int getRandomID()
        {
            int id;

            do id = rand.Next(); while (reservationsCalendar.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        //TODO: maybe move add and remove to calendar day
        private void addToDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay.Date == reservation.time.Date);
            if (resDay == null)
            {
                resDay = new CalendarDay() {theDay = reservation.time.Date};
                reservationsCalendar.Add(resDay);
            }

            resDay.reservations.Add(reservation);
            resDay.calculateReservedSeats();
            //resDay.reservedSeats += reservation.numPeople;

        }

        private void removeFromDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);
            resDay.reservations.Remove(reservation);
            resDay.calculateReservedSeats();
            //resDay.reservedSeats -= reservation.numPeople;
        }

        public void addRoom(Room room)
        {
            rooms.Add(room);
        }

        public void removeRoom(Room room)
        {
            rooms.Remove(room);
            foreach (var day in reservationsCalendar)
            {
                day.unreserveRoom(this, room);
                //day.roomsReserved.Remove(room);
                //day.calculateSeats(this);
            }

        }

        public void changeRoom(Room oldroom, Room room)
        {
            rooms[rooms.IndexOf(oldroom)] = room;

            foreach (var day in reservationsCalendar)
            {
                int roomindex = day.roomsReserved.IndexOf(oldroom);
                if (roomindex < 0) continue;

                day.roomsReserved[roomindex] = room;
                day.calculateSeats(this); //TODO: add to add and remove room
            }
        }

        public override void save()
        {
            saveFile("reservationsCalendar", reservationsCalendar);
            saveFile("rooms", rooms);
        }



        public override void load()
        {
            reservationsCalendar = loadFile<CalendarDay>("reservationsCalendar");
            rooms = loadFile<Room>("rooms");
        }

    }
}