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

        private string path;

        public Authentication authentication;

        public ReservationController reservationController;
        public EventsController eventsController;
        public GamesController gamesController;
        public ProductsController productsController;

        public Dictionary<string, DateTime> timestamps;
        private JsonSerializer jsonSerializer = new JsonSerializer();

        public DiceServer(string path)
        {
            this.path = path;

            loadTimestamps();

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

        private const string nameext = "timestamp.json";
        private string timestampPath;

        protected void loadTimestamps()
        {
            Dictionary<string, DateTime> content = null;
            timestampPath = path + "/data/";

            Directory.CreateDirectory(timestampPath);
            try
            {
                using (StreamReader streamReader = new StreamReader(timestampPath + nameext))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    content = jsonSerializer.Deserialize<Dictionary<string, DateTime>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("timestamps not found"); // TODO: put this stuff inside some function
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

            using (StreamWriter streamWriter = new StreamWriter(timestampPath + nameext))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, timestamps);
            }

        }
    }
}