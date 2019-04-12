using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace SuperNoughtsAndCrosses.Test
{
    public class E2ETest : IDisposable
    {
        private readonly IWebHost _host;
        private readonly ChromeDriver _webDriver;
        
        private readonly string _baseAddress;

        public E2ETest()
        {
            _host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(@"/Users/pivotal/workspace/superNoughtsAndCrosses/SuperNoughtsAndCrosses/")
                .UseStartup<Startup>()
                .Build();
            _host.Start();
            
            _baseAddress = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();

            _webDriver = new ChromeDriver {Url = _baseAddress + "/game"};
        }

        [Fact]
        public void NineBoardGameFlow()
        {
            var boards = _webDriver.FindElementsByClassName("board");
            Assert.Equal(9, boards.Count);
            
            var tiles = _webDriver.FindElementsByClassName("tile");
            Assert.Equal(81, tiles.Count);
            
            var gameOver = _webDriver.FindElementsByClassName("game-over");
            Assert.Empty(gameOver);

            PlayInPosition(0, 0, 0, 0, "X");
            Assert.True(BoardMarkedAsChosen(0,0));
            Assert.Equal(1, NumberOfChosenBoards());
            
            // Tries to play in wrong board
            FailToPlayInPosition(2, 2, 1, 1);
            
            PlayInPosition(0, 0, 1, 0, "O");
            PlayInPosition(1, 0, 1, 1, "X");
            PlayInPosition(1, 1, 0, 0, "O");
            PlayInPosition(0, 0, 1, 1, "X");
            PlayInPosition(1, 1, 1, 1, "O");
            PlayInPosition(1, 1, 0, 1, "X");
            PlayInPosition(0, 1, 0, 0, "O");
            WinBoardByPlayingInPosition(0, 0, 2, 2, "X");

            PlayInPosition(2, 2, 0, 0, "O");
            Assert.False(BoardMarkedAsChosen(0,0));
            Assert.Equal(8, NumberOfChosenBoards());
            
            // Can play in any board
            PlayInPosition(2, 1, 2, 2, "X");
            PlayInPosition(2, 2, 0, 1, "O");
            PlayInPosition(0, 1, 1, 1, "X");
            WinBoardByPlayingInPosition(1, 1, 2, 2, "O");

            PlayInPosition(2, 2, 0, 2, "X");
            PlayInPosition(0, 2, 1, 0, "O");
            PlayInPosition(1, 0, 0, 1, "X");
            PlayInPosition(0, 1, 1, 0, "O");
            WinBoardByPlayingInPosition(1, 0, 2, 1, "X");
            
            PlayInPosition(2, 1, 2, 0, "O");
            PlayInPosition(2, 0, 0, 2, "X");
            PlayInPosition(0, 2, 2, 0, "O");
            PlayInPosition(2, 0, 1, 2, "X");
            PlayInPosition(1, 2, 2, 0, "O");
            WinBoardByPlayingInPosition(2, 0, 2, 2, "X");
            
            Assert.Equal(0, NumberOfChosenBoards());

            gameOver = _webDriver.FindElementsByClassName("game-over");
            Assert.Equal(1, gameOver.Count);
            Assert.Contains("Game Over. CROSS has won.", gameOver.First().Text);
            
            FailToPlayInPosition(2, 2, 2, 2);
        }

        public void Dispose()
        {
            _host.StopAsync();
            _webDriver.Quit();
        }

        private void WinBoardByPlayingInPosition(int boardRow, int boardCol, int tileRow, int tileCol,
            string expectedPlayerSymbol)
        {
            var tile = FindTileAtPosition(boardRow, boardCol, tileRow, tileCol);
            tile.Click();
            
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => 
                _webDriver.FindElement(By.CssSelector($".board-row-{boardRow} > .board-{boardCol}")).Text.Equals(expectedPlayerSymbol));
        }

        private void PlayInPosition(int boardRow, int boardCol, int tileRow, int tileCol, string expectedPlayerSymbol)
        {
            var tile = FindTileAtPosition(boardRow, boardCol, tileRow, tileCol);
            tile.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => 
                FindTileAtPosition(boardRow, boardCol, tileRow, tileCol).Text.Contains(expectedPlayerSymbol));
        }

        private void FailToPlayInPosition(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            var tile = FindTileAtPosition(boardRow, boardCol, tileRow, tileCol);
            tile.Click();

            Thread.Sleep(3 * 1000);
            var tileAfterText = FindTileAtPosition(boardRow, boardCol, tileRow, tileCol).Text;
            Assert.True(tileAfterText.Equals(""));

        }
        
        private IWebElement FindTileAtPosition(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            return _webDriver.FindElement(By.CssSelector(
                $".board-row-{boardRow} > .board-{boardCol} > .row-{tileRow} > .tile-{tileCol}"));
        }

        private bool BoardMarkedAsChosen(int boardRow, int boardCol)
        {
            return _webDriver.FindElement(By.CssSelector($".board-row-{boardRow} > .board-{boardCol}")).GetAttribute("class")
                .Contains("board-chosen");
        }

        private int NumberOfChosenBoards()
        {
            return _webDriver.FindElementsByClassName("board-chosen").Count;
        }
    }
}