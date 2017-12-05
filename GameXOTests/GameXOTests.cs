using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamesLib;

namespace GameXOTests
{
    [TestClass]
    public class GameXOTests
    {
        private IGame game; //= new GameXO();

        [TestInitialize]
        public void Initialize()
        {
            game = new GameXO();
            (game as GameXO).StartGame(1, "1", "2");
        }

        [DataRow("0,0", "0,1", "0,2")]
        [DataRow("1,0", "1,1", "1,2")]
        [DataRow("2,0", "2,1", "2,2")]
        [DataRow("0,0", "1,1", "2,2")]
        [DataRow("0,0", "1,0", "2,0")]
        [DataRow("0,1", "1,1", "2,1")]
        [DataRow("0,2", "1,2", "2,2")]
        [DataRow("0,0", "1,1", "2,2")]
        [DataRow("0,2", "1,1", "2,0")]
        [DataTestMethod]
        public void WinCondTests(string a1, string a3, string a2)
        {
            (game as GameXO).SetTest();
            game.Action("1", a1);
            game.Action("1", a2);
            game.Action("1", a3);
            Assert.AreEqual(GameState.EndWithWinner, game.CurrentState);
            Assert.AreEqual("1", game.Winner);
        }

        [TestMethod]
        public void DrawCondTest()
        {
            game.Action("1", "0,0");
            game.Action("2", "0,1");

            game.Action("1", "1,0");
            game.Action("2", "1,1");

            game.Action("1", "0,2");
            game.Action("2", "2,0");

            game.Action("1", "1,2");
            game.Action("2", "2,2");

            game.Action("1", "2,1");

            Assert.AreEqual(GameState.EndWithDraw, game.CurrentState);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WrongUserTrunExTest()
        {
            game.Action("1", "0,0");
            string s = game.CurrentState.ToString();
            game.Action("1", "0,2");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WrongCellExTest()
        {
            game.Action("1", "0,0");
            game.Action("2", "0,0");
        }
    }
}
