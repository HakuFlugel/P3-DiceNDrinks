using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using Shared;

namespace AdministratorPanel {
    class XmlParser {
        private string searchPre = "http://geekdo.com/xmlapi/search?search=";
        private string searchSuf = "&exact=1";
        private XmlDocument xmlDocument = new XmlDocument();

        public List<Game> gameSearchResult = new List<Game>();
        
        public XmlParser() { }
        
        public List<Game> getGames(string searchWord) {
            xmlDocument.Load(searchPre + searchWord + searchSuf);
            gameSearchResult = PreLoadListOfGames();
            gameSearchResult = ExpandToFullList(gameSearchResult);
            return gameSearchResult;
        }

        private List<Game> ExpandToFullList(List<Game> gameList) {

            foreach (var game in gameList) {

                XmlDocument gameXmlText = new XmlDocument();
                gameXmlText.Load("http://geekdo.com/xmlapi/boardgame/" + game.bggid);
                XmlNode node = gameXmlText.FirstChild.FirstChild;

                game.minPlayers = GetInformationInt("minplayers", node);

                game.maxPlayers = GetInformationInt("maxplayers", node);

                if (game.maxPlayers == 0) {
                    game.maxPlayers = game.minPlayers;
                }

                game.minPlayTime = GetInformationInt("minplaytime", node);

                game.maxPlayTime = GetInformationInt("Maximum playtime", node);

                if (game.maxPlayTime == 0) {
                    game.maxPlayTime = game.minPlayTime;
                }

                game.description = GetInformationString("description", node);

                game.imageName = GetInformationString("thumbnail", node);
                
            }
            return gameList;
        }

        private int GetInformationInt(string field, XmlNode node) {
            int value;
            try {
                Int32.TryParse(node[field].InnerText, out value);
            } catch (Exception) {
                value = 0;
            }
            return value;
        }

        private string GetInformationString(string field, XmlNode node) {
            string value;
            try {
                value = stringFormater(node[field].InnerText);
            } catch (Exception) {
               value = "";
            }
            return value;
        }

        private List<Game> PreLoadListOfGames() {
            List<Game> temp = new List<Game>();

            foreach (XmlNode item in xmlDocument.FirstChild.ChildNodes) {
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
