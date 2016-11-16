using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class ReservationController
    {
        private List<Room> rooms;
        public List<CalendarDay> calDayList = new List<CalendarDay>();



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

            public UpdateReservationEventArgs(Reservation oldReservation, Reservation reservation1, bool hasMoved)
            {
                this.oldReservation = oldReservation;
                this.reservation = reservation;
                this.hasMoved = hasMoved;
            }
        }

        public string asJSon()
        {
            return null;
        }

        private Random rand = new Random();
        public int getRandomID()
        {
            int id;

            do id = rand.Next(); while (calDayList.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        public void addReservation(Reservation reservation)
        {
            reservation.id = getRandomID();

            //CalendarDay resDay = calDayList.First(o => o.theDay == reservation.time);

            //resDay.reservations.Add(reservation);
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
                CalendarDay resDay = calDayList.First(o => o.theDay == reservation.time.Date);

                resDay.reservations[resDay.reservations.IndexOf(oldReservation)] = reservation;
            }


            ReservationUpdated?.Invoke(this, new UpdateReservationEventArgs(oldReservation, reservation, hasMoved));

        }

        public void addToDay(Reservation reservation)
        {
            CalendarDay resDay = calDayList.FirstOrDefault(o => o.theDay == reservation.time.Date);
            if (resDay == null)
            {
                resDay = new CalendarDay() {theDay = reservation.time.Date};
                calDayList.Add(resDay);
            }

            resDay.reservations.Add(reservation);

        }

        public void removeFromDay(Reservation reservation)
        {
            CalendarDay resDay = calDayList.First(o => o.theDay == reservation.time.Date);
            resDay.reservations.Remove(reservation);
        }

    }
}