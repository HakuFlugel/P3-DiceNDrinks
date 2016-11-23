using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared {
    class Genres {
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

            Directory.CreateDirectory("Sources");
            var json = JsonConvert.SerializeObject(games);
            Directory.CreateDirectory("Sources");
            if (!File.Exists(@"Sources/Games.json"))
                File.Create(@"Sources/Games.json");

            using (StreamWriter textWriter = new StreamWriter(@"Sources/Games.json")) {
                foreach (var item in json) {
                    textWriter.Write(item.ToString());
                }
            }
        }

        public void Load() {
            string input;
            if (File.Exists(@"Sources/Games.json")) {
                using (StreamReader streamReader = new StreamReader(@"Sources/Games.json")) {
                    input = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                if (input != null) {
                    games = JsonConvert.DeserializeObject<List<Game>>(input);

                }
            }
        }

        public void Save() {

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
