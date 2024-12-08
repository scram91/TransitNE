using Microsoft.AspNetCore.Mvc.Testing;


namespace TransitNETesting.Tests.UnitTests
{
    public class PageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PageTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Privacy")]
        [InlineData("/SeptaRouteInformation/Septa")]
        [InlineData("/NJTransitRouteInformation/NJTransit")]
        [InlineData("/PatcoRouteInformation/Patco")]
        [InlineData("/SeptaRouteMap/SeptaRouteMap")]
        [InlineData("/PatcoRouteMap/PatcoRouteMap")]
        [InlineData("/NJTransitRouteMap/NJTransitRouteMap")]
        [InlineData("/TripPlanner/Map")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Register")]
        [InlineData("/Identity/Account/RegisterConfirmation")]
        [InlineData("/Identity/Account/ForgotPassword")]
        [InlineData("/Identity/Account/ResetPasswordConfirmation")]
        [InlineData("/Identity/Account/ResendEmailConfirmation")]
        [InlineData("/Identity/Account/Lockout")]
        [InlineData("/Identity/Account/ForgotPasswordConfirmation")]
        [InlineData("/Identity/Account/ExternalLogin")]
        [InlineData("/Identity/Account/ConfirmEmail")]
        [InlineData("/Identity/Account/ConfirmEmailChange")]
        [InlineData("/Identity/Account/AccessDenied")]
        [InlineData("/Identity/Account/Manage/Index")]
        [InlineData("/Identity/Account/Manage/ExternalLogins")]
        [InlineData("/Identity/Account/Manage/ShowRecoveryCodes")]
        [InlineData("/Identity/Account/Manage/ResetAuthenticator")]
        [InlineData("/Identity/Account/Manage/Email")]
        [InlineData("/Identity/Account/Manage/ChangePassword")]
        [InlineData("/Identity/Account/Manage/DeletePersonalData")]

        public async Task AllPagesLoad(string URL)
        {
            //Arrange
            var client = _factory.CreateClient();
            //Act
            var response = await client.GetAsync(URL);
            int code = (int)response.StatusCode;
            //Assert
            Assert.Equal(200, code);
        }
    }
}