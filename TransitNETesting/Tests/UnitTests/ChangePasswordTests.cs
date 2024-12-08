using System.Collections.Generic;
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
    public class ChangePasswordModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _mockUserManager;
        private readonly Mock<SignInManager<TransitNEUser>> _mockSignInManager;
        private readonly Mock<ILogger<ChangePasswordModel>> _mockLogger;

        public ChangePasswordModelTests()
        {
            _mockUserManager = new Mock<UserManager<TransitNEUser>>(
                Mock.Of<IUserStore<TransitNEUser>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<TransitNEUser>>(
                _mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<TransitNEUser>>(),
                null, null, null, null);

            _mockLogger = new Mock<ILogger<ChangePasswordModel>>();
        }

        [Fact]
        public async Task OnGetAsync_UserWithoutPassword_RedirectsToSetPassword()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.HasPasswordAsync(user)).ReturnsAsync(false);
            
            var pageModel = new ChangePasswordModel(_mockUserManager.Object, _mockSignInManager.Object, _mockLogger.Object);

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./SetPassword", redirectResult.PageName);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageResult()
        {
            // Arrange
            var pageModel = new ChangePasswordModel(_mockUserManager.Object, _mockSignInManager.Object, _mockLogger.Object);
            pageModel.ModelState.AddModelError("NewPassword", "Password is required");

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_InvalidOldPassword_AddsModelError_ReturnsPage()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.ChangePasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Incorrect old password." }));

            var pageModel = new ChangePasswordModel(_mockUserManager.Object, _mockSignInManager.Object, _mockLogger.Object)
            {
                Input = new ChangePasswordModel.InputModel
                {
                    OldPassword = "wrongpassword",
                    NewPassword = "newPassword123!",
                    ConfirmPassword = "newPassword123!"
                }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.True(pageModel.ModelState.ContainsKey(string.Empty));
            Assert.Contains("Incorrect old password.", pageModel.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task OnPostAsync_ValidPasswordChange_RedirectsToPage()
        {
            // Arrange
            var user = new TransitNEUser { UserName = "testuser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(m => m.ChangePasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockSignInManager.Setup(m => m.RefreshSignInAsync(user)).Returns(Task.CompletedTask);

            var pageModel = new ChangePasswordModel(_mockUserManager.Object, _mockSignInManager.Object, _mockLogger.Object)
            {
                Input = new ChangePasswordModel.InputModel
                {
                    OldPassword = "oldPassword123!",
                    NewPassword = "newPassword123!",
                    ConfirmPassword = "newPassword123!"
                }
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Null(redirectResult.PageName);
            Assert.Equal("Your password has been changed.", pageModel.StatusMessage);
        }
    }
}
