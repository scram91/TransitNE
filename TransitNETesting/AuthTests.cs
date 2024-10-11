using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TransitNETesting
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public AuthTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Fact]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            //Arange 
            var client = _factory.CreateClient(
               new WebApplicationFactoryClientOptions
               {
                    AllowAutoRedirect = false
               });

            // Act
            var response = await client.GetAsync("/SecurePage");

            // Assert
            //Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login",
            response.Headers.Location.OriginalString);
        }
    }
}
