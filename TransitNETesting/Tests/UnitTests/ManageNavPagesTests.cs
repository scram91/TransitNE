using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TransitNE.Areas.Identity.Pages.Account.Manage;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class ManageNavPagesTests
    {
        private readonly Mock<ViewContext> _mockViewContext;

        public ManageNavPagesTests()
        {
            _mockViewContext = new Mock<ViewContext>();
        }

        [Fact]
        public void PageNavClass_ReturnsActive_ForCorrectPage()
        {
            // Arrange
            var activePage = "Index";
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns(activePage);

            // Act
            var result = ManageNavPages.PageNavClass(_mockViewContext.Object, "Index");

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void PageNavClass_ReturnsNull_ForIncorrectPage()
        {
            // Arrange
            var activePage = "Email";
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns(activePage);

            // Act
            var result = ManageNavPages.PageNavClass(_mockViewContext.Object, "Index");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IndexNavClass_ReturnsActive_ForIndexPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("Index");

            // Act
            var result = ManageNavPages.IndexNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void EmailNavClass_ReturnsActive_ForEmailPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("Email");

            // Act
            var result = ManageNavPages.EmailNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void ChangePasswordNavClass_ReturnsActive_ForChangePasswordPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("ChangePassword");

            // Act
            var result = ManageNavPages.ChangePasswordNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void TwoFactorAuthenticationNavClass_ReturnsActive_ForTwoFactorAuthenticationPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("TwoFactorAuthentication");

            // Act
            var result = ManageNavPages.TwoFactorAuthenticationNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void PersonalDataNavClass_ReturnsActive_ForPersonalDataPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("PersonalData");

            // Act
            var result = ManageNavPages.PersonalDataNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void ExternalLoginsNavClass_ReturnsActive_ForExternalLoginsPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("ExternalLogins");

            // Act
            var result = ManageNavPages.ExternalLoginsNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void DeletePersonalDataNavClass_ReturnsActive_ForDeletePersonalDataPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("DeletePersonalData");

            // Act
            var result = ManageNavPages.DeletePersonalDataNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }

        [Fact]
        public void DownloadPersonalDataNavClass_ReturnsActive_ForDownloadPersonalDataPage()
        {
            // Arrange
            _mockViewContext.Setup(v => v.ViewData["ActivePage"]).Returns("DownloadPersonalData");

            // Act
            var result = ManageNavPages.DownloadPersonalDataNavClass(_mockViewContext.Object);

            // Assert
            Assert.Equal("active", result);
        }
    }
}
