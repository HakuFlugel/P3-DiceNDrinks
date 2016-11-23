using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared {
    public class Genres {
        public List<string> differentGenres = new List<string> {
            "Horror",
            "Lying",
            "Other stuff",
            "Third stuff",
            "Strategy",
            "Coop",
            "Adventure",
            "dnd",
            "Entertainment",
            "Comic",
            "Ballzy",
            "#360NoScope" };


        public Genres() {
            Load();
           
        }

        public void Load() {
            string input;
            if (File.Exists(@"Sources/Genres.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/Genres.json")) {
                    input = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                if (input != null) {
                    differentGenres = JsonConvert.DeserializeObject<List<string>>(input);

                }
            }
        }

        public void Save(){
            Directory.CreateDirectory("Sources");
            var json = JsonConvert.SerializeObject(differentGenres);
            Directory.CreateDirectory("Sources");


            using (StreamWriter textWriter = new StreamWriter(@"Sources/Genres.json")) {
                foreach (var item in json) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public void add(string add) {
            differentGenres.Add(add);
        }

        public void remove(string remove) {
            differentGenres.Remove(remove);
        }

        public void rename(string before, string after) {
            remove(before);
            add(after);
        }
    }
}
