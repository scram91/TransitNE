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
        
        [Fact(Skip = "Moved Test to theory")]
        public async Task TestRouteInformation()
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync("/RouteInformation/Index");
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/RouteInformation/Index")]
        [InlineData("/Ticketing/Index")]
        [InlineData("/TripPlanner/Index")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Register")]
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