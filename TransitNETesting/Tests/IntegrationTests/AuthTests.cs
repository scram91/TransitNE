using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TransitNETesting.Tests;
using System.Net;
using System.Net.Http.Headers;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace TransitNETesting.Tests.IntegrationTests;

public class AuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthTests()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _factory = factory;
    }

    [Theory]
    [InlineData("/Customers")]
    [InlineData("/Customers/Details")]
    [InlineData("/Customers/Details/1")]
    [InlineData("/Customers/Edit")]
    [InlineData("/Customers/Edit/1")]
    public async Task Get_EndpointsReturnFailToAnonymousUserForRestrictedUrls(string url)
    {
        // Arrange
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // Act
        using var response = await client.GetAsync(url);
        var redirectUrl = response.Headers.Location.LocalPath;

        // Assert
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal("/auth/login", redirectUrl);
    }

}