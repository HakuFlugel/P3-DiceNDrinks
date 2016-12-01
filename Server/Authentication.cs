using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    public class Authentication
    {

        private Dictionary<string, string> adminKeys;

        private readonly string path;
        private const string nameext = "adminkeys.json";

        protected  JsonSerializer jsonSerializer = JsonSerializer.Create();

        public Authentication(string path)
        {
            this.path = path;
        }

        public bool authenticate(string key)
        {
            return adminKeys.ContainsKey(key);
        }

        public void load()
        {

            Directory.CreateDirectory(path);
            try
            {
                using (StreamReader streamReader = new StreamReader(path + nameext))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    adminKeys = jsonSerializer.Deserialize<Dictionary<string, string>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(nameext + " not found"); // TODO: put this stuff inside some function
            }

            if (adminKeys == null)
            {
                Console.WriteLine(nameext + " was null after loading... setting it to new list");
                adminKeys = new Dictionary<string, string>();
            }
        }

        public void save()
        {
            using (StreamWriter streamWriter = new StreamWriter(path + nameext))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, content);
            }

        }

    }
}