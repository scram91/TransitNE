using Microsoft.AspNetCore.Mvc.Testing;


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
        [InlineData("/SeptaRouteInformation/Septa")]
        [InlineData("/NJTransitRouteInformation/NJTransit")]
        [InlineData("/PatcoRouteInformation/Patco")]
        [InlineData("/RouteMap/RouteMap")]
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