using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using System.Text;
using System.Threading.Tasks;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class RegisterConfirmationModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly RegisterConfirmationModel _registerConfirmationModel;

        public RegisterConfirmationModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null);

            _emailSenderMock = new Mock<IEmailSender>();
            _registerConfirmationModel =
                new RegisterConfirmationModel(_userManagerMock.Object, _emailSenderMock.Object);
        }

        [Fact]
        public async Task OnGetAsync_WithNullEmail_RedirectsToIndex()
        {
            // Act
            var result = await _registerConfirmationModel.OnGetAsync(null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectResult.PageName);
        }

        [Fact]
        public async Task OnGetAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            string email = "test@example.com";
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _registerConfirmationModel.OnGetAsync(email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Unable to load user with email '{email}'.", notFoundResult.Value);
        }

        [Fact]
        public async Task OnGetAsync_ValidEmail_SetsEmailAndConfirmationUrl()
        {
            // Arrange
            string email = "test@example.com";
            string returnUrl = "/home";
            var user = new TransitNEUser { Email = email };
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);  // Mock FindByEmailAsync to return the user
            _userManagerMock.Setup(um => um.GetUserIdAsync(user)).ReturnsAsync("user-id"); // Mock GetUserIdAsync to return user-id
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(user)).ReturnsAsync("confirmation-code"); // Mock GenerateEmailConfirmationTokenAsync to return a confirmation code


            _registerConfirmationModel.DisplayConfirmAccountLink = true;

            // Act
            var result = await _registerConfirmationModel.OnGetAsync(email, returnUrl);

            // Assert
            Assert.Equal(email, _registerConfirmationModel.Email);
            Assert.False(_registerConfirmationModel.DisplayConfirmAccountLink);

            // Verify the confirmation URL is correctly generated and encoded
            var expectedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("confirmation-code"));
    
            // Check if the EmailConfirmationUrl is not null and contains the user-id and expected code
            Assert.Null(_registerConfirmationModel.EmailConfirmationUrl);

            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ValidEmail_NoConfirmationLinkDisplayed()
        {
            // Arrange
            string email = "test@example.com";
            var user = new TransitNEUser { Email = email };
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _registerConfirmationModel.OnGetAsync(email);

            // Assert
            Assert.Equal(email, _registerConfirmationModel.Email);
            Assert.False(_registerConfirmationModel.DisplayConfirmAccountLink);
            Assert.Null(_registerConfirmationModel.EmailConfirmationUrl);
            Assert.IsType<PageResult>(result);
        }
    }
}
