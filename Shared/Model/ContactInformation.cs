using System;

namespace Shared {
    [Serializable]
    public class ContactInformation {
        public string description;
        public string email;
        public string number;
        public string[] location;
        public string geoURI;
        public string[] openingTimes;
    }
}
