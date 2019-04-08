using System;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AngleSharp;
using AngleSharp.Dom;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace SuperNoughtsAndCrosses.Test
{
    public class GameControllerTest :
        IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private IWebDriver _driver;

        private readonly WebApplicationFactory<Startup> _factory;

        public GameControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _driver = new ChromeDriver();
        }

        [Theory]
        [InlineData("/game")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GameEndpoint_DisplaysTheBoard()
        {
            var client = _factory.CreateClient();

            var response = await client.GetStringAsync("/game");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var tiles = htmlDoc.DocumentNode.SelectNodes("//*[@class='tile']");

            Assert.NotNull(tiles);
            Assert.Equal(9, tiles.Count);
        }

        [Fact]
        public async Task GameEndpoint_TileChangesToCrossWhenClicked()
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync("/game");
          
            var content = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var tile = (IHtmlElement) content.QuerySelector(".row-2 > .tile-2");
            
            Assert.NotNull(tile);
            Assert.Equal("", tile.TextContent);
            
            tile.DoClick();

            var tileAfterClick = (IHtmlElement) content.QuerySelector(".row-2 > .tile-2");
            
            Assert.Equal("X", tileAfterClick.TextContent);
            
        }

        [Fact]
        public void GoToGamePage()
        {
            _driver.Manage().Window.Maximize();
            
            var client = _factory.CreateClient();
            _driver.Navigate().GoToUrl("http://localhost:" + client.BaseAddress.Port + "/game");
            
            Assert.Equal("Super Noughts and Crosses", _driver.Title);
        }

        public void Dispose()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception e)
            {
                Console.Write($"Exception while stopping chrome... {e}");
            }
        }
    }
}