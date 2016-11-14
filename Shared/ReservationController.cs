using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class ReservationController
    {
        private List<Room> rooms;
        public List<CalendarDay> calDayList = new List<CalendarDay>();


        public string asJSon()
        {
            return null;
        }

        private Random rand = new Random();
        public int getRandomID()
        {
            int id;

            do id = rand.Next(1, int.MaxValue); while (calDayList.Any(cdl => cdl.reservations.Exists(res => res.id == id)));

            return id;

        }

        public void submitReservation(Reservation reservation) // TODO: split into add and edit?
        {
            if (reservation.id == 0)
            {
                //TODO: get next id
            }

            CalendarDay resDay = calDayList.First(o => o.theDay == reservation.time);
            Reservation res = resDay.reservations.First();

        }

    }
}