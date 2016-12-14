using System;

namespace Shared {
    [Serializable]
    public class Event {
        public int id;
        public DateTime timestamp;
        public string name;
        public string description;
        public string facebookID; /*TODO: Facebook stuff*/
        public DateTime startDate;
        public DateTime endDate;
    }
}