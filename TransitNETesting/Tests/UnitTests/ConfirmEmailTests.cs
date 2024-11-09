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
    public class ConfirmEmailModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly ConfirmEmailModel _confirmEmailModel;

        public ConfirmEmailModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null, null);

            _confirmEmailModel = new ConfirmEmailModel(_userManagerMock.Object);
        }

        [Fact]
        public async Task OnGetAsync_NullUserIdOrCode_RedirectsToIndex()
        {
            // Act
            var result = await _confirmEmailModel.OnGetAsync(null, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectResult.PageName);
        }

        [Fact]
        public async Task OnGetAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "nonexistent-user";
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _confirmEmailModel.OnGetAsync(userId, "valid-code");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task OnGetAsync_ValidUserAndCorrectCode_ConfirmsEmailAndDisplaysSuccessMessage()
        {
            // Arrange
            var user = new TransitNEUser { Id = "valid-user" };
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("valid-code"));

            _userManagerMock.Setup(um => um.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ConfirmEmailAsync(user, "valid-code")).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _confirmEmailModel.OnGetAsync(user.Id, code);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Thank you for confirming your email.", _confirmEmailModel.StatusMessage);
        }

        [Fact]
        public async Task OnGetAsync_ValidUserAndIncorrectCode_DisplaysErrorMessage()
        {
            // Arrange
            var user = new TransitNEUser { Id = "valid-user" };
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes("invalid-code"));

            _userManagerMock.Setup(um => um.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ConfirmEmailAsync(user, "invalid-code"))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _confirmEmailModel.OnGetAsync(user.Id, code);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("Error confirming your email.", _confirmEmailModel.StatusMessage);
        }
    }
}
