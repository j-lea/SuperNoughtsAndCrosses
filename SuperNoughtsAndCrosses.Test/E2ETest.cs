using System;
using System.Linq;
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
            
            _webDriver = new ChromeDriver();
        }

        [Fact]
        public void PlayBasicGame()
        {
            _webDriver.Url = _baseAddress + "/game";
            
            var tiles = _webDriver.FindElementsByClassName("tile");
            Assert.Equal(9, tiles.Count);

            
            PlayInPosition(1, 1, "X");
            PlayInPosition(0, 2, "O");
            PlayInPosition(1, 2, "X");
            
            // Play in position again and nothing happens
            PlayInPosition(1, 2, "X");
           
            PlayInPosition(0, 0, "O");
        }
      

        public void Dispose()
        {
            _host.StopAsync();
            _webDriver.Quit();
        }

        private void PlayInPosition(int row, int col, string playerSymbol)
        {
            var tile = FindTileAtPosition(row, col);
            tile.Click();

            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(3));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(d => FindTileAtPosition(row, col).Text.Contains(playerSymbol));
        }

        private IWebElement FindTileAtPosition(int row, int column)
        {
            return _webDriver.FindElement(By.CssSelector($".row-{row} > .tile-{column}"));
        }
    }
}