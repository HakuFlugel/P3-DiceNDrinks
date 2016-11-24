using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shared {
    public class Genres {
        public List<string> differentGenres = new List<string>();


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
            } else
                addtemplates();
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

        private void addtemplates() {
            differentGenres.Add("Horror");
            differentGenres.Add("Strategy");
            differentGenres.Add("Horror");
            differentGenres.Add("Family Friendly");
            differentGenres.Add("Coop");
            differentGenres.Add("Challenge");
            differentGenres.Add("Competetive");
            differentGenres.Add("RPG");
            differentGenres.Add("Cardgame");
            differentGenres.Add("Casual");
            differentGenres.Add("Casual");
            differentGenres.Add("Pary game");
            differentGenres.Add("Logic game");
            differentGenres.Add("Educational game");
            differentGenres.Add("War game");
            differentGenres.Add("Abstracts");
            differentGenres.Add("Thematic Games");
        }
    }
}
