using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Encodings.Web;

namespace TransitNETesting.Tests.IntegrationTests;

public class AuthTests : AuthenticationHandler<AuthenticationSchemeOptions>
{
    [Fact]
    public async Task get_SecurePageRedirectsAnUnauthenticatedUser()
    {
        //Arange
        var client = _applicationFactory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        //Act
        var response = await client.GetAsync("/SecurePage");

        //Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/Identity/Account/Login",
            response.Headers.Location.OriginalString);
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }
}