using Microsoft.AspNetCore.Mvc.RazorPages;
using TransitNE.Areas.Identity.Pages.Account;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class LockoutTests
    {
        [Fact]
        public void OnGet_ReturnsPage()
        {
            // Arrange
            var model = new LockoutModel();

            // Act
            model.OnGet();

            // Assert
            Assert.IsType<PageModel>(model);
        }
    }
}