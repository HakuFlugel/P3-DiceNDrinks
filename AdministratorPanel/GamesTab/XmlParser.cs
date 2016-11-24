using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using Shared;

namespace AdministratorPanel {
    class XmlParser {
        string search = "http://geekdo.com/xmlapi/search?search=";
        string searchend = "&exact=1";
        private XmlDocument doc = new XmlDocument();

        public List<Game> gameSearchResult = new List<Game>();
        
        public XmlParser() {
            
        }
        
        public List<Game> getGames(string ind) {
            doc.Load(search + ind + searchend);
            gameSearchResult = PreLoadListOfGames();
            gameSearchResult = ExpandToFullList(gameSearchResult);

            return gameSearchResult;
        }

        private List<Game> ExpandToFullList(List<Game> gameList) {

            foreach (var game in gameList) {

                XmlDocument gameXmlText = new XmlDocument();
                gameXmlText.Load("http://geekdo.com/xmlapi/boardgame/" + game.bggid);
                XmlNode node = gameXmlText.FirstChild.FirstChild;

                //info

                int intHolder; bool boolHolder;

                Dictionary<string, bool> thisWasHere = new Dictionary<string, bool>();

                try {
                    Int32.TryParse(node["minplayers"].InnerText, out intHolder);
                    boolHolder = true;
                } catch (Exception) {
                    intHolder = 0;
                    boolHolder = false;
                }

                thisWasHere.Add("Minimum players", boolHolder);
                game.minPlayers = intHolder;

                try {
                    Int32.TryParse(node["maxplayers"].InnerText, out intHolder);
                    boolHolder = true;
                } catch (Exception) {
                    intHolder = 0;
                    boolHolder = false;
                }

                thisWasHere.Add("Maximum players", boolHolder);
                game.maxPlayers = intHolder;

                try {
                    Int32.TryParse(node["minplaytime"].InnerText, out intHolder);
                    boolHolder = true;
                } catch (Exception) {
                    intHolder = 0;
                    boolHolder = false;
                }
                thisWasHere.Add("Minimum playtime", boolHolder);
                game.minPlayTime = intHolder;

                try {
                    Int32.TryParse(node["maxplaytime"].InnerText, out intHolder);
                    boolHolder = true;
                } catch (Exception) {
                    intHolder = 0;
                    boolHolder = false;
                }
                thisWasHere.Add("Maximum playtime", boolHolder);
                game.maxPlayTime = intHolder;

                try {
                    game.description = stringFormater(node["description"].InnerText);
                    boolHolder = true;
                } catch (Exception) {
                    game.description = "";
                    boolHolder = false;
                }
                thisWasHere.Add("Description", boolHolder);

                try {
                    game.thumbnail = node["thumbnail"].InnerText;
                    boolHolder = true;
                } catch (Exception) {
                    game.thumbnail = "";
                    boolHolder = false;
                }

                thisWasHere.Add("Thumbnail", boolHolder);

                try {
                    game.imageName = node["image"].InnerText;
                    boolHolder = true;
                } catch {
                    game.imageName = "";
                    boolHolder = false;
                }
                thisWasHere.Add("Image", boolHolder);
                bool textBoxNeeded = false;
                string whatIsMissing = "These things could not be found in game with ID: " + game.bggid +  " from the API: " + Environment.NewLine + Environment.NewLine;
                foreach (var item in thisWasHere.Where(x => x.Value.Equals(false))) {
                    whatIsMissing += "  * " + item.Key + Environment.NewLine;
                    textBoxNeeded = true;
                }
                string caption = (game.name != "") ? game.name : "Game with unknown name.";
                if (textBoxNeeded) {
                    //MessageBox.Show(whatIsMissing, caption);
                }
                    
            }


            return gameList;
        }

        private List<Game> PreLoadListOfGames() {
            List<Game> temp = new List<Game>();
            int amount = doc.ChildNodes.Count;

            foreach (XmlNode item in doc.FirstChild.ChildNodes) {
                Game game = new Game();
                game.bggid = item.Attributes["objectid"].Value;
                game.name = item["name"].InnerText;
                game.publishedYear = Convert.ToInt32(item["yearpublished"].InnerText);

                temp.Add(game);

            }
            return temp;
        }

        public string stringFormater(string input) {

            input = Regex.Replace(input, @"<[^>]*>", String.Empty);
            input = Regex.Replace(input, @"&quot;", "\"");
            input = Regex.Replace(input, @"&mdash;", String.Empty);
            input = Regex.Replace(input, @"&times;", "*");

            return input;
        }
    }
}
