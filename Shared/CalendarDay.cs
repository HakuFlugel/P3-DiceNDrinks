using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class CalendarDay
    {
        public List<Reservation> reservations = new List<Reservation>();
        public int reservedSeats;
        public int numSeats;
        public bool isFullChecked;
        public HashSet<Room> roomsReserved; // TODO: room fulness, reference? duplicate?
        public DateTime theDay;

        public void calculateSeats(ReservationController reservationController)
        {
            numSeats = reservationController.rooms.Where(o => !roomsReserved.Contains(o)).Sum(o => o.seats);
        }
    }
}