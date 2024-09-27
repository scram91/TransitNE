using Microsoft.AspNetCore.Mvc;
using TransitNETesting.Tests;
using TransitNE.Controllers;
using System.Security.Policy;
using System;

namespace TransitNETesting.Tests.ControllerTests
{
    public class HomeControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly CustomWebApplicationFactory<Program> _customWebApplicationFactory;

        public HomeControllerTests()
        {
            var factory = new CustomWebApplicationFactory<Program>();
            _customWebApplicationFactory = factory;
            _httpClient = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [Theory]
        [InlineData("/Home/Index")]
        public async Task TestHomeIndexViewAsync(string URL)
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
        [InlineData("/Home/Privacy")]
        public async Task TestHomePrivacyViewAsync(string URL)
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