using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using TransitNETesting.Utilities.Helpers;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{

    public class ConfirmEmailChangeModelTests
    {
        private Mock<IUserStore<TransitNEUser>> _userStoreMock;
        private UserManager<TransitNEUser> _userManager;
        private SignInManager<TransitNEUser> _signInManager;

        [Fact]
        public async Task OnGetAsync_MissingParameters_ReturnsRedirectToIndex()
        {
            // Arrange
            _userManager = IdentityTestHelpers.GetTestUserManager();
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);

            var model = new ConfirmEmailChangeModel(_userManager, _signInManager);

            // Act
            var result = await model.OnGetAsync(null, null, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectResult.PageName);
        }

        [Fact]
        public async Task OnGetAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _userManager = IdentityTestHelpers.GetTestUserManager();
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);

            // User not found scenario
            _userStoreMock.Setup(s => s.FindByIdAsync("testUserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync((TransitNEUser)null);

            var model = new ConfirmEmailChangeModel(_userManager, _signInManager);

            // Act
            var result = await model.OnGetAsync("testUserId", "test@example.com", "validCode");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Unable to load user with ID 'testUserId'.", notFound.Value);
        }

        [Fact]
        public async Task OnGetAsync_EmailChangeFails_ReturnsPageWithError()
        {
            // Arrange
            _userManager = IdentityTestHelpers.GetTestUserManager();
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);

            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };

            _userStoreMock.Setup(s => s.FindByIdAsync("testUserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Mock ChangeEmailAsync to fail
            var userManagerMock = new Mock<UserManager<TransitNEUser>>(
                _userStoreMock.Object,
                null, null, new List<IUserValidator<TransitNEUser>>(),
                new List<IPasswordValidator<TransitNEUser>>(),
                null, null, null, null
            );
            
            userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error changing email" }));

            // Replace _userManager with our userManagerMock.Object so the method calls are mocked
            _userManager = userManagerMock.Object;
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);
            var model = new ConfirmEmailChangeModel(_userManager, _signInManager);

            // Act
            var result = await model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Error changing email.", model.StatusMessage);
        }

        [Fact]
        public async Task OnGetAsync_UserNameChangeFails_ReturnsPageWithError()
        {
            // Arrange
            _userManager = IdentityTestHelpers.GetTestUserManager();
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);

            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };
            _userStoreMock.Setup(s => s.FindByIdAsync("testUserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var userManagerMock = new Mock<UserManager<TransitNEUser>>(
                _userStoreMock.Object,
                null, null, new List<IUserValidator<TransitNEUser>>(),
                new List<IPasswordValidator<TransitNEUser>>(),
                null, null, null, null
            );

            // Success on ChangeEmailAsync
            userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Fail on SetUserNameAsync
            userManagerMock.Setup(um => um.SetUserNameAsync(user, "new@example.com"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error changing user name" }));

            _userManager = userManagerMock.Object;
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);
            var model = new ConfirmEmailChangeModel(_userManager, _signInManager);

            // Act
            var result = await model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Error changing user name.", model.StatusMessage);
        }

        [Fact]
        public async Task OnGetAsync_SuccessfulEmailChange_ReturnsPageWithSuccessMessage()
        {
            // Arrange
            _userManager = IdentityTestHelpers.GetTestUserManager();
            _signInManager = IdentityTestHelpers.GetTestSignInManager(_userManager);

            var user = new TransitNEUser { Id = "testUserId", Email = "old@example.com" };
            _userStoreMock.Setup(s => s.FindByIdAsync("testUserId", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var userManagerMock = new Mock<UserManager<TransitNEUser>>(
                _userStoreMock.Object,
                null, null, new List<IUserValidator<TransitNEUser>>(),
                new List<IPasswordValidator<TransitNEUser>>(),
                null, null, null, null
            );

            // Success on ChangeEmailAsync and SetUserNameAsync
            userManagerMock.Setup(um => um.ChangeEmailAsync(user, "new@example.com", It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(um => um.SetUserNameAsync(user, "new@example.com"))
                .ReturnsAsync(IdentityResult.Success);

            // Mock RefreshSignInAsync
            var signInManagerMock = new Mock<SignInManager<TransitNEUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<TransitNEUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<TransitNEUser>>().Object
            );
            signInManagerMock.Setup(sm => sm.RefreshSignInAsync(user))
                .Returns(Task.CompletedTask);

            _userManager = userManagerMock.Object;
            _signInManager = signInManagerMock.Object;
            var model = new ConfirmEmailChangeModel(_userManager, _signInManager);

            // Act
            var result = await model.OnGetAsync("testUserId", "new@example.com", "validCode");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Thank you for confirming your email change.", model.StatusMessage);
        }
    }
}

