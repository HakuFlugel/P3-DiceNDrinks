
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Shared
{
    public abstract class ControllerBase
    {
        private string path;// = "data/";
        private const string ext = ".json";

        private  JsonSerializer jsonSerializer = JsonSerializer.Create();

        public abstract void save();
        public abstract void load();

        protected ControllerBase(string path = "data/")
        {
            this.path = path;
        }

        protected List<T> loadFile<T>(string name)
        {
            List<T> content = null;

            Directory.CreateDirectory(path);
            try
            {
                using (StreamReader streamReader = new StreamReader(path + name + ext))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    content = jsonSerializer.Deserialize<List<T>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(name + " not found"); // TODO: put this stuff inside some function
            }

            if (content == null)
            {
                Console.WriteLine(name + " was null after loading... setting it to new list");
                content = new List<T>();
            }

            return content;
        }

        protected void saveFile<T>(string name, List<T> content)
        {
            Directory.CreateDirectory(path);

            using (StreamWriter streamWriter = new StreamWriter(path + name + ext))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, content);
            }

        }


    }
}