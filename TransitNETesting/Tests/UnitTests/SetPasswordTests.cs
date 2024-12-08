using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TransitNE.Areas.Identity.Pages.Account.Manage;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class SetPasswordModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _mockUserManager;
        private SignInManager<TransitNEUser> _mockSignInManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IUserClaimsPrincipalFactory<TransitNEUser>> _mockUserClaimsPrincipalFactory;
        private readonly SetPasswordModel _model;

        public SetPasswordModelTests()
        {
            // Mock the dependencies of SignInManager
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            _mockUserClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>();

            // Create the mock UserManager and SignInManager
            _mockUserManager = new Mock<UserManager<TransitNEUser>>(
                Mock.Of<IUserStore<TransitNEUser>>(), // Mock the IUserStore<TransitNEUser>
                null, // Mock the IUserValidator<TransitNEUser>
                null, // Mock the IPasswordValidator<TransitNEUser>
                null, // Mock the IPasswordHasher<TransitNEUser>
                null, // Mock the IUserRoleStore<TransitNEUser>
                null, // Mock the IUserClaimStore<TransitNEUser>
                null, // Mock the IUserLoginStore<TransitNEUser>
                null, // Mock the IUserTokenProvider<TransitNEUser>
                null // Mock or leave null for IdentityOptions
            );

            // Manually instantiate SignInManager with the mocked dependencies
            _mockSignInManager = new SignInManager<TransitNEUser>(
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object,
                _mockUserClaimsPrincipalFactory.Object,
                Mock.Of<IOptions<IdentityOptions>>(), // Mock or pass an empty instance
                Mock.Of<ILogger<SignInManager<TransitNEUser>>>(), // Mock logger
                Mock.Of<IAuthenticationSchemeProvider>(), // Mock scheme provider
                null // Add any other necessary null or mocked dependencies
            );

            // Initialize the SetPasswordModel with the mocks
            _model = new SetPasswordModel(_mockUserManager.Object, _mockSignInManager);

        }
        

        [Fact]
        public async Task OnGetAsync_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                            .ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _model.OnGetAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Unable to load user with ID ''.", notFoundResult.Value);
        }

        [Fact]
        public async Task OnGetAsync_RedirectsToChangePassword_WhenUserHasPassword()
        {
            // Arrange
            var user = new TransitNEUser();
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.HasPasswordAsync(user)).ReturnsAsync(true);

            // Act
            var result = await _model.OnGetAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./ChangePassword", redirectResult.PageName);
        }

        [Fact]
        public async Task OnGetAsync_ReturnsPage_WhenUserDoesNotHavePassword()
        {
            // Arrange
            var user = new TransitNEUser();
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.HasPasswordAsync(user)).ReturnsAsync(false);

            // Act
            var result = await _model.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsPage_WhenModelStateIsInvalid()
        {
            // Arrange
            _model.Input = new SetPasswordModel.InputModel(); // Empty input, which is invalid
            _model.ModelState.AddModelError("NewPassword", "Required");

            // Act
            var result = await _model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsPage_WhenAddPasswordFails()
        {
            // Arrange
            _model.Input = new SetPasswordModel.InputModel { NewPassword = "NewPassword123", ConfirmPassword = "NewPassword123" };
            var user = new TransitNEUser();
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.AddPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error adding password" }));

            // Act
            var result = await _model.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Contains("Error adding password", _model.ModelState[""].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task OnPostAsync_SetsPasswordSuccessfully()
        {
            // Arrange
            _model.Input = new SetPasswordModel.InputModel { NewPassword = "NewPassword123", ConfirmPassword = "NewPassword123" };
            var user = new TransitNEUser();
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.AddPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _model.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./SetPassword", redirectResult.PageName);
            Assert.Equal("Your password has been set.", _model.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync((TransitNEUser)null);

            // Act
            var result = await _model.OnPostAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Unable to load user with ID ''.", notFoundResult.Value);
        }
    }
}
