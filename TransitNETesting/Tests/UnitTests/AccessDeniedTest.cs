using Microsoft.AspNetCore.Mvc.RazorPages;
using TransitNE.Areas.Identity.Pages.Account;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class AccessDeniedModelTests
    {
        [Fact]
        public void OnGet_ReturnsPage()
        {
            // Arrange
            var model = new AccessDeniedModel();

            // Act
            model.OnGet();

            // Assert
            Assert.IsType<AccessDeniedModel>(model);
        }
    }
}
