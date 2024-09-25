using Microsoft.AspNetCore.Mvc.Testing;

namespace TransitNETesting.Tests.IntegrationTests;

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
    [InlineData("/Home/Privacy")]
    [InlineData("/RouteInformation/Septa")]
    [InlineData("/RouteInformation/NJTransit")]
    [InlineData("/RouteInformation/Patco")]
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

    [Theory]
    [InlineData("/")]
    [InlineData("/Home/Index")]
    [InlineData("/Home/Privacy")]
    [InlineData("/RouteInformation/Septa")]
    [InlineData("/RouteInformation/NJTransit")]
    [InlineData("/RouteInformation/Patco")]
    [InlineData("/Ticketing/Index")]
    [InlineData("/TripPlanner/Index")]
    [InlineData("/Identity/Account/Login")]
    [InlineData("/Identity/Account/Register")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string URL)
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync(URL);

        //Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}