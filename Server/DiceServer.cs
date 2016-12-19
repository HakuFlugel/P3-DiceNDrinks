using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Shared;

// In case we use Application['string'] to store loaded data, remember to lock and unlock before modifying it...


namespace Server
{
    public class DiceServer
    {

        public string path { get; private set; }

        public Authentication authentication;

        public ReservationController reservationController;
        public EventsController eventsController;
        public GamesController gamesController;
        public ProductsController productsController;

        public Dictionary<string, DateTime> timestamps;
        public ContactInformation contactInformation;

        private JsonSerializer jsonSerializer = new JsonSerializer();

        public DiceServer(string path)
        {
            this.path = path;

            loadTimestamps();
            loadContactInformation();

            authentication = new Authentication(path+"data/");
            authentication.load();

            reservationController = new ReservationController(path+"data/");
            reservationController.load();
            eventsController = new EventsController(path+"data/");
            eventsController.load();
            gamesController = new GamesController(path+"data/");
            gamesController.load();
            productsController = new ProductsController(path+"data/");
            productsController.load();

        }


        private const string contactinfoNameExt = "contactinfo.json";
        private string contactinfoPath;
        private void loadContactInformation()
        {
            ContactInformation content = null;
            contactinfoPath = path + "/data/";

            Directory.CreateDirectory(contactinfoPath);
            try
            {
                using (StreamReader streamReader = new StreamReader(contactinfoPath + contactinfoNameExt))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    content = jsonSerializer.Deserialize<ContactInformation>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("contactinfo not found");
            }

            if (content == null)
            {
                Console.WriteLine("contactinfo was null after loading... setting it to new object");
                content = new ContactInformation();
            }

            contactInformation = content;
        }

        protected void saveContactInformation()
        {
            Directory.CreateDirectory(contactinfoPath);

            using (StreamWriter streamWriter = new StreamWriter(contactinfoPath + contactinfoNameExt))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, contactInformation);
            }

        }

        public void setTimestamp(string key, DateTime timestamp)
        {
            if (!timestamps.ContainsKey(key))
            {
                timestamps.Add(key, timestamp);
            }
            else if (timestamp >= timestamps[key])
            {
                timestamps[key] = timestamp;

            }

            saveTimestamps();

        }

        private const string timestampNameExt = "timestamp.json";
        private string timestampPath;

        protected void loadTimestamps()
        {
            Dictionary<string, DateTime> content = null;
            timestampPath = path + "/data/";

            Directory.CreateDirectory(timestampPath);
            try
            {
                using (StreamReader streamReader = new StreamReader(timestampPath + timestampNameExt))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    content = jsonSerializer.Deserialize<Dictionary<string, DateTime>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("timestamps not found");
            }

            if (content == null)
            {
                Console.WriteLine("timestamps was null after loading... setting it to new list");
                content = new Dictionary<string, DateTime>();
            }

            timestamps = content;
        }

        protected void saveTimestamps()
        {
            Directory.CreateDirectory(timestampPath);

            using (StreamWriter streamWriter = new StreamWriter(timestampPath + timestampNameExt))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, timestamps);
            }

        }
    }
}