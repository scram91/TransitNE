using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TransitNE.Controllers;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly Mock<UserManager<TransitNEUser>> _mockUserManager;
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();

        // Set up UserManager mock with a fake user store
        var userStore = new Mock<IUserStore<TransitNEUser>>();
        _mockUserManager = new Mock<UserManager<TransitNEUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        // Set up a fake user ID for testing
        _mockUserManager.Setup(m => m.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("test-user-id");

        _controller = new HomeController(_mockLogger.Object, _mockUserManager.Object);
    }

    [Fact]
    public void Index_Returns_View_And_Sets_UserID()
    {
        // Arrange
        // Mocking the controller's user. This sets a principal with an identity.
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }, "TestAuthType"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = userClaims
            }
        };

        // Act
        var result = _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-user-id", _controller.ViewData["UserID"]);
        Assert.Equal(string.Empty, result.ViewName ?? string.Empty); 
        // If no view name is specified, ASP.NET Core uses the action name as the view name by default.
    }

    [Fact]
    public void Privacy_Returns_View()
    {
        // Act
        var result = _controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.ViewName ?? string.Empty);
    }
}
}
