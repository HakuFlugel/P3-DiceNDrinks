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
        public List<Room> roomsReserved; // TODO: room fulness, reference? duplicate?
        public DateTime theDay;

        public void reserveRoom(ReservationController reservationController, Room room)
        {
            roomsReserved.Add(room);
            calculateSeats(reservationController);
        }

        public void unreserveRoom(ReservationController reservationController, Room room)
        {
            roomsReserved.Remove(room);
            calculateSeats(reservationController);
        }

        public void calculateSeats(ReservationController reservationController)
        {
            numSeats = reservationController.rooms.Sum(r => r.seats);//reservationController.rooms.Where(o => !roomsReserved.Contains(o)).Sum(o => o.seats);
            numSeats -= roomsReserved.Sum(r => r.seats);
        }
    }
}