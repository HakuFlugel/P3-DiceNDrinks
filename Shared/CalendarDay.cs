using System;
using System.Collections.Generic;

namespace Shared
{
    public class CalendarDay
    {
        public List<Reservation> reservations;
        public int fullness;
        public bool isFullChecked;
        public List<bool> roomsReserved; // TODO: room fulness, reference? duplicate?
        public DateTime theDay;
    }
}