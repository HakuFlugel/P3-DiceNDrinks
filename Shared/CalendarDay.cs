using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public class CalendarDay
    {
        public DateTime timestamp;
        public List<Reservation> reservations = new List<Reservation>();
        public int reservedSeats;
        public int reservedSeatsPending;
        public int numSeats;
        public bool isLocked = false;
        public bool isAutoaccept = true;
        public int defaultAcceptPresentage = 50;
        public int acceptPresentage;
        public int defaultAcceptMaxPeople = 5;
        public int autoAcceptMaxPeople;
        public bool isFullChecked;
        public List<Room> roomsReserved = new List<Room>(); // TODO: room fulness, reference? duplicate?
        public DateTime theDay;

        public CalendarDay() {
            acceptPresentage = defaultAcceptPresentage;
            autoAcceptMaxPeople = defaultAcceptMaxPeople;
        }
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
            //numSeats = reservationController.rooms.Sum(r => r.seats);//reservationController.rooms.Where(o => !roomsReserved.Contains(o)).Sum(o => o.seats);
            numSeats = reservationController.totalSeats - roomsReserved.Sum(r => r.seats);
        }

        public void calculateReservedSeats()
        {
            reservedSeats = reservations.Where(r => r.state == Reservation.State.Accepted).Sum(r => r.numPeople);
            reservedSeatsPending = reservations.Where(r => r.state == Reservation.State.Pending).Sum(r => r.numPeople);
        }
    }
}