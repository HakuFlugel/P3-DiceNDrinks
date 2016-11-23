using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

// TODO: reservation af lokale osv. : Vi har List<int> med de rum der har her. Har List<int> på hver dag, hvor de gennem en checkbox eller lignende kan tilføje/fjerne en sådan reservation

namespace Shared
{
    public class ReservationController : ControllerBase
    {
        JsonSerializer jsonSerializer = JsonSerializer.Create();

        public List<Room> rooms = new List<Room>();
        public List<CalendarDay> reservationsCalendar = new List<CalendarDay>();

        public event EventHandler<AddReservationEventArgs> ReservationAdded;

        public class AddReservationEventArgs
        {
            public Reservation reservation;

            public AddReservationEventArgs(Reservation reservation)
            {
                this.reservation = reservation;
            }
        }

        public event EventHandler<UpdateReservationEventArgs> ReservationUpdated;

        public class UpdateReservationEventArgs
        {
            public Reservation reservation;
            public Reservation oldReservation;
            public bool hasMoved;

            public UpdateReservationEventArgs(Reservation oldReservation, Reservation reservation, bool hasMoved)
            {
                this.oldReservation = oldReservation;
                this.reservation = reservation;
                this.hasMoved = hasMoved;
            }
        }

        public event EventHandler<RemoveReservationEventArgs> ReservationRemoved;

        public class RemoveReservationEventArgs
        {
            public Reservation reservation;

            public RemoveReservationEventArgs(Reservation reservation)
            {
                this.reservation = reservation;
            }
        }

        public void addReservation(Reservation reservation)
        {
            reservation.id = getRandomID();
            reservation.created = DateTime.Now;

            addToDay(reservation);

            ReservationAdded?.Invoke(this, new AddReservationEventArgs(reservation));

        }

        public void updateReservation(Reservation oldReservation, Reservation reservation)
        {
            bool hasMoved = reservation.time.Date != oldReservation.time.Date;
            //TODO: do we need this if the if-else is commented out?

//            if (hasMoved)
//            {
            removeFromDay(oldReservation);
            addToDay(reservation);
//            }
//            else
//            {
//                CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);
//
//                resDay.reservations[resDay.reservations.IndexOf(oldReservation)] = reservation; // This would have to update reservedseats too, but why split this responsibility even more?
//            }


            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(oldReservation, reservation, hasMoved));

        }

        public void removeReservation(Reservation reservation)
        {

            removeFromDay(reservation);

            ReservationRemoved?.Invoke(this, new RemoveReservationEventArgs(reservation));

        }

        private Random rand = new Random();

        private int getRandomID()
        {
            int id;

            do id = rand.Next(); while (reservationsCalendar.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        private void addToDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay == reservation.time.Date);
            if (resDay == null)
            {
                resDay = new CalendarDay() {theDay = reservation.time.Date};
                reservationsCalendar.Add(resDay);
            }

            resDay.reservations.Add(reservation);
            resDay.calculateSeats(this);
            //resDay.reservedSeats += reservation.numPeople;

        }

        private void removeFromDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);
            resDay.reservations.Remove(reservation);
            resDay.calculateSeats(this);
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
                day.calculateSeats(this);
            }
        }

        public override void save()
        {
            Directory.CreateDirectory("data");

            using (StreamWriter streamWriter = new StreamWriter("data/reservationsCalendar.json"))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, reservationsCalendar);
            }
            using (StreamWriter streamWriter = new StreamWriter("data/rooms.json"))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, rooms);
            }
        }



        public override void load()
        {
            //TODO: create a function for these(consider Entity System thing)
            Directory.CreateDirectory("data");
            try
            {
                using (StreamReader streamReader = new StreamReader("data/reservationsCalendar.json"))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    reservationsCalendar = jsonSerializer.Deserialize<List<CalendarDay>>(jsonTextReader);

                }
                using (StreamReader streamReader = new StreamReader("data/rooms.json"))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    rooms = jsonSerializer.Deserialize<List<Room>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("reservationsCalendar.json or rooms.json not found"); // TODO: put this stuff inside some function
            }

            if (reservationsCalendar == null)
            {
                Console.WriteLine("reservationsCalendar was null after loading... setting it to new list");
                reservationsCalendar = new List<CalendarDay>();
            }

            if (rooms == null)
            {
                Console.WriteLine("rooms was null after loading... setting it to new list");
                rooms = new List<Room>();
            }

        }

    }
}