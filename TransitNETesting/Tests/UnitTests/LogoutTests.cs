using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class LogoutModelTests
    {
        private readonly Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private readonly Mock<ILogger<LogoutModel>> _loggerMock;
        private readonly LogoutModel _logoutModel;

        public LogoutModelTests()
        {
            _signInManagerMock = new Mock<SignInManager<TransitNEUser>>(
                new Mock<UserManager<TransitNEUser>>(
                        new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null)
                    .Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>().Object,
                null, null, null, null);

            _loggerMock = new Mock<ILogger<LogoutModel>>();
            _logoutModel = new LogoutModel(_signInManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task OnPost_LogsOutUserAndLogsInformation()
        {
            // Arrange
            _signInManagerMock.Setup(sm => sm.SignOutAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _logoutModel.OnPost();

            // Assert
            _signInManagerMock.Verify(sm => sm.SignOutAsync(), Times.Once);
            _loggerMock.Verify(log => log.LogInformation("User logged out."), Times.Once);
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPost_WithReturnUrl_RedirectsToReturnUrl()
        {
            // Arrange
            string returnUrl = "/somepage";
            _signInManagerMock.Setup(sm => sm.SignOutAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _logoutModel.OnPost(returnUrl);

            // Assert
            var redirectResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal(returnUrl, redirectResult.Url);
        }

        [Fact]
        public async Task OnPost_WithoutReturnUrl_RedirectsToPage()
        {
            // Arrange
            _signInManagerMock.Setup(sm => sm.SignOutAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _logoutModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
