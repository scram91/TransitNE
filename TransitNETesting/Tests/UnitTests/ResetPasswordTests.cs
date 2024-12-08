using Microsoft.AspNetCore.Identity;
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
    public class ResetPasswordModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly ResetPasswordModel _resetPasswordModel;

        public ResetPasswordModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null);

            _resetPasswordModel = new ResetPasswordModel(_userManagerMock.Object);
        }

        [Fact]
        public void OnGet_NullCode_ReturnsBadRequest()
        {
            // Act
            var result = _resetPasswordModel.OnGet(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A code must be supplied for password reset.", badRequestResult.Value);
        }

        [Fact]
        public void OnGet_ValidCode_SetsInputModelAndReturnsPage()
        {
            // Arrange
            string code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("test-code"));

            // Act
            var result = _resetPasswordModel.OnGet(code);

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("test-code", _resetPasswordModel.Input.Code);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
        {
            // Arrange
            _resetPasswordModel.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _resetPasswordModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_UserNotFound_RedirectsToResetPasswordConfirmation()
        {
            // Arrange
            _resetPasswordModel.Input = new ResetPasswordModel.InputModel { Email = "nonexistent@example.com" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _resetPasswordModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./ResetPasswordConfirmation", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostAsync_ResetPasswordSucceeded_RedirectsToResetPasswordConfirmation()
        {
            // Arrange
            var user = new TransitNEUser { Email = "existing@example.com" };
            _resetPasswordModel.Input = new ResetPasswordModel.InputModel
            {
                Email = user.Email,
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                Code = "valid-code"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ResetPasswordAsync(user, "valid-code", "NewPassword123!"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _resetPasswordModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./ResetPasswordConfirmation", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostAsync_ResetPasswordFailed_ReturnsPageWithErrors()
        {
            // Arrange
            var user = new TransitNEUser { Email = "existing@example.com" };
            _resetPasswordModel.Input = new ResetPasswordModel.InputModel
            {
                Email = user.Email,
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                Code = "valid-code"
            };

            var identityErrors = new IdentityError[]
            {
                new IdentityError { Description = "Password must be at least 6 characters long." },
                new IdentityError { Description = "Password must contain a number." }
            };
            var identityResult = IdentityResult.Failed(identityErrors);

            _userManagerMock.Setup(um => um.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ResetPasswordAsync(user, "valid-code", "NewPassword123!"))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _resetPasswordModel.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Contains("Password must be at least 6 characters long.",
                _resetPasswordModel.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.Contains("Password must contain a number.",
                _resetPasswordModel.ModelState[string.Empty].Errors[1].ErrorMessage);
        }
    }
}
