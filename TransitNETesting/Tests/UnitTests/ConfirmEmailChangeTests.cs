using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Moq;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class ConfirmEmailChangeModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private readonly ConfirmEmailChangeModel _model;

        public ConfirmEmailChangeModelTests()
        {
            _userManagerMock = new Mock<UserManager<TransitNEUser>>(MockBehavior.Strict,
                new Mock<IUserStore<TransitNEUser>>().Object, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<TransitNEUser>>(MockBehavior.Strict,
                _userManagerMock.Object, null, null, null, null, null);

            _model = new ConfirmEmailChangeModel(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task OnGetAsync_MissingParameters_ReturnsRedirectToIndex()
        {
            // Act
            var result = await _model.OnGetAsync(null, null, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectResult.PageName);
        }

        [Fact]
        public async Task OnGetAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userManagerMock.Setup(um => um.FindByIdAsync("testUserId")).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _model.OnGetAsync("testUserId", "test@example.com", "validCode");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Unable to load user with ID 'testUserId'.", notFoundResult.Value);
        }

        [Fact]
        public async Task OnGetAsync_EmailChangeFails_ReturnsPageWithError()
        {
            // Arrange
            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("testUserId")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", "validCode"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error changing email" }));

            // Act
            var result = await _model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Error changing email.", _model.StatusMessage);
        }

        [Fact]
        public async Task OnGetAsync_UserNameChangeFails_ReturnsPageWithError()
        {
            // Arrange
            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("testUserId")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", "validCode"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.SetUserNameAsync(user, "new@example.com"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error changing user name" }));

            // Act
            var result = await _model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Error changing user name.", _model.StatusMessage);
        }

        [Fact]
        public async Task OnGetAsync_SuccessfulEmailChange_ReturnsPageWithSuccessMessage()
        {
            // Arrange
            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync("testUserId")).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", "validCode"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.SetUserNameAsync(user, "new@example.com"))
                .ReturnsAsync(IdentityResult.Success);
            _signInManagerMock.Setup(sm => sm.RefreshSignInAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Thank you for confirming your email change.", _model.StatusMessage);
        }
    }
}
