using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class ForgotPasswordModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly ForgotPasswordModel _forgotPasswordModel;

        public ForgotPasswordModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null);
            _emailSenderMock = new Mock<IEmailSender>();

            _forgotPasswordModel = new ForgotPasswordModel(_userManagerMock.Object, _emailSenderMock.Object);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
        {
            // Arrange
            _forgotPasswordModel.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _forgotPasswordModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_UserNotFoundOrEmailNotConfirmed_RedirectsToForgotPasswordConfirmation()
        {
            // Arrange
            _forgotPasswordModel.Input = new ForgotPasswordModel.InputModel { Email = "nonexistent@example.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _forgotPasswordModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./ForgotPasswordConfirmation", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostAsync_ValidUserAndConfirmedEmail_SendsEmailAndRedirectsToConfirmation()
        {
            // Arrange
            var user = new TransitNEUser { Email = "confirmed@example.com" };
            _forgotPasswordModel.Input = new ForgotPasswordModel.InputModel { Email = user.Email };

            _userManagerMock.Setup(um => um.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset-token");

            // Act
            var result = await _forgotPasswordModel.OnPostAsync();

            // Assert
            _emailSenderMock.Verify(es => es.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    It.Is<string>(body => body.Contains("clicking here"))),
                Times.Once);

            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./ForgotPasswordConfirmation", redirectResult.PageName);
        }
    }
}
