using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using TransitNE.Areas.Identity.Pages.Account.Manage;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class EmailModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _mockUserManager;
        private readonly Mock<SignInManager<TransitNEUser>> _mockSignInManager;
        private readonly Mock<IEmailSender> _mockEmailSender;

        public EmailModelTests()
        {
            _mockUserManager = new Mock<UserManager<TransitNEUser>>(
                Mock.Of<IUserStore<TransitNEUser>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<TransitNEUser>>(
                _mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<TransitNEUser>>(),
                null, null, null, null);

            _mockEmailSender = new Mock<IEmailSender>();
        }

        [Fact]
        public async Task OnGetAsync_UserExists_LoadsUserEmailAndStatus()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetEmailAsync(user)).ReturnsAsync(user.Email);
            _mockUserManager.Setup(m => m.IsEmailConfirmedAsync(user)).ReturnsAsync(true);

            var pageModel = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailSender.Object);

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("test@example.com", pageModel.Email);
            Assert.True(pageModel.IsEmailConfirmed);
        }

        [Fact]
        public async Task OnPostChangeEmailAsync_ValidEmail_SendsConfirmationEmail()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", Email = "old@example.com" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetEmailAsync(user)).ReturnsAsync("old@example.com");
            _mockUserManager.Setup(m => m.GetUserIdAsync(user)).ReturnsAsync("testUserId");
            _mockUserManager.Setup(m => m.GenerateChangeEmailTokenAsync(user, "new@example.com"))
                .ReturnsAsync("testCode");

            var pageModel = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailSender.Object)
            {
                Input = new EmailModel.InputModel { NewEmail = "new@example.com" }
            };

            // Act
            var result = await pageModel.OnPostChangeEmailAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Confirmation link to change email sent. Please check your email.", pageModel.StatusMessage);
            _mockEmailSender.Verify(m => m.SendEmailAsync(
                "new@example.com",
                "Confirm your email",
                It.Is<string>(s => s.Contains("Please confirm your account"))),
                Times.Once);
        }

        [Fact]
        public async Task OnPostChangeEmailAsync_EmailUnchanged_ShowsUnchangedStatusMessage()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetEmailAsync(user)).ReturnsAsync("test@example.com");

            var pageModel = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailSender.Object)
            {
                Input = new EmailModel.InputModel { NewEmail = "test@example.com" }
            };

            // Act
            var result = await pageModel.OnPostChangeEmailAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your email is unchanged.", pageModel.StatusMessage);
        }

        [Fact]
        public async Task OnPostSendVerificationEmailAsync_ValidUser_SendsVerificationEmail()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetEmailAsync(user)).ReturnsAsync("test@example.com");
            _mockUserManager.Setup(m => m.GetUserIdAsync(user)).ReturnsAsync("testUserId");
            _mockUserManager.Setup(m => m.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync("testCode");

            var pageModel = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailSender.Object);

            // Act
            var result = await pageModel.OnPostSendVerificationEmailAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Verification email sent. Please check your email.", pageModel.StatusMessage);
            _mockEmailSender.Verify(m => m.SendEmailAsync(
                "test@example.com",
                "Confirm your email",
                It.Is<string>(s => s.Contains("Please confirm your account"))),
                Times.Once);
        }

        [Fact]
        public async Task OnPostChangeEmailAsync_InvalidModelState_ReturnsPageResult()
        {
            // Arrange
            var pageModel = new EmailModel(_mockUserManager.Object, _mockSignInManager.Object, _mockEmailSender.Object);
            pageModel.ModelState.AddModelError("Input.NewEmail", "New email is required");

            // Act
            pageModel.OnPostChangeEmailAsync();

            // Assert
            Assert.IsType<EmailModel>(pageModel);
        }
    }
}

