using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SuperNoughtsAndCrosses.Test
{
    public class GameControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public GameControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
    }
}