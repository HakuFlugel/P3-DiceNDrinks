
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Shared
{
    public abstract class ControllerBase
    {

        protected  JsonSerializer jsonSerializer = JsonSerializer.Create();

        public abstract void save();
        public abstract void load();

        protected List<T> loadFile<T>(string path)
        {
            List<T> content = null;

            Directory.CreateDirectory("data");
            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    content = jsonSerializer.Deserialize<List<T>>(jsonTextReader);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(path + " not found"); // TODO: put this stuff inside some function
            }

            if (content == null)
            {
                Console.WriteLine(path + " was null after loading... setting it to new list");
                content = new List<T>();
            }

            return content;
        }

        protected void saveFile<T>(string path, List<T> content)
        {
            Directory.CreateDirectory("data");

            using (StreamWriter streamWriter = new StreamWriter(path))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonSerializer.Serialize(jsonTextWriter, content);
            }

        }


    }
}