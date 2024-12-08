using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class ResendEmailConfirmationModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly ResendEmailConfirmationModel _resendEmailConfirmationModel;

        public ResendEmailConfirmationModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null);

            _emailSenderMock = new Mock<IEmailSender>();
            _resendEmailConfirmationModel =
                new ResendEmailConfirmationModel(_userManagerMock.Object, _emailSenderMock.Object);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
        {
            // Arrange
            _resendEmailConfirmationModel.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _resendEmailConfirmationModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_UserNotFound_ReturnsPageWithModelError()
        {
            // Arrange
            _resendEmailConfirmationModel.Input = new ResendEmailConfirmationModel.InputModel
                { Email = "nonexistent@example.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _resendEmailConfirmationModel.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(_resendEmailConfirmationModel.ModelState.ContainsKey(string.Empty));
            Assert.Contains("Verification email sent. Please check your email.",
                _resendEmailConfirmationModel.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task OnPostAsync_UserFound_SendsEmailAndReturnsPageWithModelError()
        {
            // Arrange
            var user = new TransitNEUser { Email = "existing@example.com" };
            _resendEmailConfirmationModel.Input = new ResendEmailConfirmationModel.InputModel { Email = user.Email };

            _userManagerMock.Setup(um => um.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetUserIdAsync(user)).ReturnsAsync("user-id");
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(user))
                .ReturnsAsync("confirmation-code");

            // Act
            var result = await _resendEmailConfirmationModel.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(_resendEmailConfirmationModel.ModelState.ContainsKey(string.Empty));
            Assert.Contains("Verification email sent. Please check your email.",
                _resendEmailConfirmationModel.ModelState[string.Empty].Errors[0].ErrorMessage);

            // Verify email sending
            _emailSenderMock.Verify(es => es.SendEmailAsync(
                    user.Email,
                    "Confirm your email",
                    It.Is<string>(msg => msg.Contains(HtmlEncoder.Default.Encode(
                        WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("confirmation-code")))))),
                Times.Once);
        }
    }
}

