using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdministratorPanel;
using Shared;


namespace NUnitTest {
    [TestFixture]
    public class Games {



 



        [Test]
        public void TestMethod() {
          
        }


        [Test]
        public void GameListTest() {
            Game game = new Game();

            List<Game> games = new List<Game>();
            GamesTab gamesTab = new GamesTab();
            Genres genres = new Genres();
            GamesList gamesList;

            games.Add(game);
            gamesList = new GamesList(games, gamesTab, genres);

            Assert.IsTrue(games.Find(x => x.name == game.name) == game && gamesList.gametab == gamesTab);
        }


    }



}
