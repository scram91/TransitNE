using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using TransitNE.Areas.Identity.Pages.Account.Manage;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class IndexModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _mockUserManager;
        private readonly Mock<SignInManager<TransitNEUser>> _mockSignInManager;

        public IndexModelTests()
        {
            _mockUserManager = new Mock<UserManager<TransitNEUser>>(
                Mock.Of<IUserStore<TransitNEUser>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<TransitNEUser>>(
                _mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<TransitNEUser>>(),
                null, null, null, null);
        }

        [Fact]
        public async Task OnGetAsync_UserExists_LoadsUserProfile()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetUserNameAsync(user)).ReturnsAsync(user.UserName);
            _mockUserManager.Setup(m => m.GetPhoneNumberAsync(user)).ReturnsAsync(user.PhoneNumber);

            var pageModel = new IndexModel(_mockUserManager.Object, _mockSignInManager.Object);

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Equal("testuser", pageModel.Username);
            Assert.Equal("1234567890", pageModel.Input.PhoneNumber);
        }

        [Fact]
        public async Task OnPostAsync_ValidPhoneNumber_UpdateProfileSuccessfully()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetPhoneNumberAsync(user)).ReturnsAsync("1234567890");
            _mockUserManager.Setup(m => m.SetPhoneNumberAsync(user, "0987654321"))
                .ReturnsAsync(IdentityResult.Success);
            _mockSignInManager.Setup(m => m.RefreshSignInAsync(user)).Returns(Task.CompletedTask);

            var pageModel = new IndexModel(_mockUserManager.Object, _mockSignInManager.Object)
            {
                Input = new IndexModel.InputModel { PhoneNumber = "0987654321" }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your profile has been updated", pageModel.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_PhoneNumberUnchanged_NoUpdateMade()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetPhoneNumberAsync(user)).ReturnsAsync("1234567890");

            var pageModel = new IndexModel(_mockUserManager.Object, _mockSignInManager.Object)
            {
                Input = new IndexModel.InputModel { PhoneNumber = "1234567890" }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Your profile has been updated", pageModel.StatusMessage);
        }

        [Fact]
        public async Task OnPostAsync_InvalidPhoneNumber_ReturnsPageResult()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var pageModel = new IndexModel(_mockUserManager.Object, _mockSignInManager.Object);
            pageModel.ModelState.AddModelError("Input.PhoneNumber", "Phone number is required");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_SetPhoneNumberFailed_ReturnsErrorMessage()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser", PhoneNumber = "1234567890" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.GetPhoneNumberAsync(user)).ReturnsAsync("1234567890");
            _mockUserManager.Setup(m => m.SetPhoneNumberAsync(user, "0987654321"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error setting phone number" }));

            var pageModel = new IndexModel(_mockUserManager.Object, _mockSignInManager.Object)
            {
                Input = new IndexModel.InputModel { PhoneNumber = "0987654321" }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Unexpected error when trying to set phone number.", pageModel.StatusMessage);
        }
    }
}
