using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System.Security.Policy;

namespace TransitNETesting
{
    public class PageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PageTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/RouteInformation/Septa")]
        [InlineData("/RouteInformation/NJTransit")]
        [InlineData("/RouteInformation/Patco")]
        [InlineData("/RouteInformation/RouteInformation")]
        [InlineData("/Ticketing/Index")]
        [InlineData("/TripPlanner/Index")]
        public async Task AllPagesLoad(string URL)
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }
    }
}