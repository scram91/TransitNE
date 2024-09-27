

namespace TransitNETesting.Tests.ControllerTests
{
    public class RouteInformationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;

        public RouteInformationControllerTests()
        {
            var factory = new CustomWebApplicationFactory<Program>();
            _customWebApplicationFactory = factory;
            _httpClient = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [Theory]
        [InlineData("/RouteInformation/Septa")]
        public async Task TestRouteInformationSeptaViewAsync(string URL)
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("/RouteInformation/NJTransit")]
        public async Task TestRouteInformationNjTransitViewAsync(string URL)
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("/RouteInformation/Patco")]
        public async Task TestRouteInformationPatcoViewAsync(string URL)
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }

        [Theory]
        [InlineData("/RouteInformation/RouteMap")]
        public async Task TestRouteInformationRouteMapViewAsync(string URL)
        {
            //Arrange
            var client = _customWebApplicationFactory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }
    }
}
