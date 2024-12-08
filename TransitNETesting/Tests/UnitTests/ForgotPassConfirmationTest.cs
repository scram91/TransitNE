using Microsoft.AspNetCore.Mvc.RazorPages;
using TransitNE.Areas.Identity.Pages.Account;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class ForgotPassConfirmationTest
    {
        [Fact]
        public void OnGet_ReturnsPage()
        {
            // Arrange
            var model = new ForgotPasswordConfirmation();

            // Act
            model.OnGet();

            // Assert
            Assert.IsType<ForgotPasswordConfirmation>(model);
        }
    }
}
