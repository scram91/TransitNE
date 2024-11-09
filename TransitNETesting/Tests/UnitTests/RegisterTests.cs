using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Http;

namespace TransitNETesting.Tests.UnitTests
{

    public class RegisterModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private readonly Mock<IUserStore<TransitNEUser>> _userStoreMock;
        private readonly Mock<IUserEmailStore<TransitNEUser>> _emailStoreMock;
        private readonly Mock<ILogger<RegisterModel>> _loggerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly RegisterModel _registerModel;

        public RegisterModelTests()
        {
            _userStoreMock = new Mock<IUserStore<TransitNEUser>>();
            _emailStoreMock = _userStoreMock.As<IUserEmailStore<TransitNEUser>>();
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(_userStoreMock.Object, null, null, null, null, null,
                null, null, null);
            _signInManagerMock = new Mock<SignInManager<TransitNEUser>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>().Object,
                null, null, null, null);
            _loggerMock = new Mock<ILogger<RegisterModel>>();
            _emailSenderMock = new Mock<IEmailSender>();

            _registerModel = new RegisterModel(
                _userManagerMock.Object,
                _userStoreMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _emailSenderMock.Object);
        }

        [Fact]
        public async Task OnGetAsync_SetsExternalLogins()
        {
            // Arrange
            var externalSchemes = new List<AuthenticationScheme>
                { new AuthenticationScheme("Google", "Google", typeof(IAuthenticationHandler)) };
            _signInManagerMock.Setup(m => m.GetExternalAuthenticationSchemesAsync()).ReturnsAsync(externalSchemes);

            // Act
            await _registerModel.OnGetAsync();

            // Assert
            Assert.Equal(externalSchemes, _registerModel.ExternalLogins);
        }

        [Fact]
        public async Task OnPostAsync_WithValidModel_CreatesUserAndSignsIn()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _signInManagerMock.Setup(sm => sm.SignInAsync(It.IsAny<TransitNEUser>(), false, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _registerModel.OnPostAsync("~/");

            // Assert
            var redirectResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal("~/", redirectResult.Url);
            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>()), Times.Once);
            _signInManagerMock.Verify(sm => sm.SignInAsync(It.IsAny<TransitNEUser>(), false, null), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithInvalidModel_ReturnsPageResult()
        {
            // Arrange
            _registerModel.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_CreatesUserAndSendsConfirmationEmail()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GetUserIdAsync(It.IsAny<TransitNEUser>())).ReturnsAsync("user-id");
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<TransitNEUser>()))
                .ReturnsAsync("confirmation-token");

            // Act
            await _registerModel.OnPostAsync("~/");

            // Assert
            _emailSenderMock.Verify(es => es.SendEmailAsync(
                    It.IsAny<string>(),
                    "Confirm your email",
                    It.Is<string>(msg => msg.Contains("Please confirm your account by <a href="))),
                Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_WithRequireConfirmedAccount_ReturnsRedirectToPageResult()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _userManagerMock.Setup(um => um.Options.SignIn.RequireConfirmedAccount).Returns(true);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _registerModel.OnPostAsync("~/");

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("RegisterConfirmation", redirectToPageResult.PageName);
        }
    }
}
