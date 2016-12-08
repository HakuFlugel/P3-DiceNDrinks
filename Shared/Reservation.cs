using System;

namespace Shared {
    public class Reservation {
        public enum State {
            Pending = 0,
            Accepted = 1,
            Denied = 2,
        }
        public int id;
        public DateTime timestamp;
        public string name;
        public string email;
        public string phone;
        public int numPeople;
        public DateTime time;
        public DateTime created;
        public State state;

        public string message;

    }
}