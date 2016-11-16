﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Shared
{
    public class ReservationController : ControllerBase
    {
        JsonSerializer jsonSerializer = JsonSerializer.Create();

        private List<Room> rooms;
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

            public UpdateReservationEventArgs(Reservation reservation)
            {
            }

            public UpdateReservationEventArgs(Reservation oldReservation, Reservation reservation, bool hasMoved)
            {
                this.oldReservation = oldReservation;
                this.reservation = reservation;
                this.hasMoved = hasMoved;
            }
        }



        public void addReservation(Reservation reservation)
        {
            reservation.id = getRandomID();

            addToDay(reservation);

            ReservationAdded?.Invoke(this, new AddReservationEventArgs(reservation));

        }

        public void updateReservation(Reservation oldReservation, Reservation reservation)
        {
            bool hasMoved = reservation.time.Date != oldReservation.time.Date;

            if (hasMoved)
            {
                removeFromDay(oldReservation);
                addToDay(reservation);
            }
            else
            {
                CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);

                resDay.reservations[resDay.reservations.IndexOf(oldReservation)] = reservation;
            }


            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(oldReservation, reservation, hasMoved));

        }

        private Random rand = new Random();
        public int getRandomID()
        {
            int id;

            do id = rand.Next(); while (reservationsCalendar.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        public void addToDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.FirstOrDefault(o => o.theDay == reservation.time.Date);
            if (resDay == null)
            {
                resDay = new CalendarDay() {theDay = reservation.time.Date};
                reservationsCalendar.Add(resDay);
            }

            resDay.reservations.Add(reservation);

        }

        public void removeFromDay(Reservation reservation)
        {
            CalendarDay resDay = reservationsCalendar.First(o => o.theDay == reservation.time.Date);
            resDay.reservations.Remove(reservation);
        }

        public override void save()
        {
            Directory.CreateDirectory("data");
            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("data/reservationsCalendar.json")), reservationsCalendar);
            jsonSerializer.Serialize(new JsonTextWriter(new StreamWriter("data/rooms.json")), rooms);
        }

        public override void load()
        {
            Directory.CreateDirectory("data");
            reservationsCalendar = jsonSerializer.Deserialize<List<CalendarDay>>(new JsonTextReader(new StreamReader("data/reservationsCalendar.json")));
            rooms = jsonSerializer.Deserialize<List<Room>>(new JsonTextReader(new StreamReader("data/rooms.json")));

        }

    }
}