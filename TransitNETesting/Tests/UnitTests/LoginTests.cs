using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;
using TransitNE.Models;
using TransitNE.Areas.Identity.Pages.Account;

namespace TransitNETesting.Tests.UnitTests
{
    public class LoginModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private readonly LoginModel _loginModel;
        private readonly DefaultHttpContext _httpContext;

        public LoginModelTests()
        {
            _httpContext = new DefaultHttpContext();

            _userManagerMock = MockUserManager();
            _signInManagerMock = MockSignInManager();

            _loginModel = new LoginModel(_signInManagerMock.Object, new LoggerFactory().CreateLogger<LoginModel>())
            {
                PageContext = new PageContext { HttpContext = _httpContext }
            };
        }

        private Mock<UserManager<TransitNEUser>> MockUserManager()
        {
            // UserManager dependencies
            var userStoreMock = new Mock<IUserStore<TransitNEUser>>();
            var optionsMock = new Mock<IOptions<IdentityOptions>>();
            var passwordHasherMock = new Mock<IPasswordHasher<TransitNEUser>>();
            var userValidators = new List<IUserValidator<TransitNEUser>>();
            var passwordValidators = new List<IPasswordValidator<TransitNEUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var loggerMock = new Mock<ILogger<UserManager<TransitNEUser>>>();

            return new Mock<UserManager<TransitNEUser>>(
                userStoreMock.Object,
                optionsMock.Object,
                passwordHasherMock.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                identityErrorDescriber.Object,
                services.Object,
                loggerMock.Object);
        }

        private Mock<SignInManager<TransitNEUser>> MockSignInManager()
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(_ => _.HttpContext).Returns(_httpContext);

            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>();
            var options = Options.Create(new IdentityOptions());
            var logger = new Mock<ILogger<SignInManager<TransitNEUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<TransitNEUser>>();

            return new Mock<SignInManager<TransitNEUser>>(
                _userManagerMock.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                options,
                logger.Object,
                schemes.Object,
                confirmation.Object);
        }

        private IServiceProvider SetupServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(sp => sp.GetService(typeof(SignInManager<TransitNEUser>))).Returns(_signInManagerMock);
            serviceProvider.Setup(sp => sp.GetService(typeof(UserManager<TransitNEUser>))).Returns(_userManagerMock);
            serviceProvider.Setup(sp => sp.GetService(typeof(IUrlHelperFactory))).Returns(MockUrlHelperFactory().Object);
            serviceProvider.Setup(sp => sp.GetService(typeof(IAuthenticationService))).Returns(new Mock<IAuthenticationService>().Object);
            return serviceProvider.Object;
        }

        private Mock<IUrlHelperFactory> MockUrlHelperFactory()
        {
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Content(It.IsAny<string>())).Returns((string path) => path);

            var urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
            urlHelperFactoryMock.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelperMock.Object);
            return urlHelperFactoryMock;
        }

        private void ConfigureHttpContext(bool isAuthenticated)
        {
            var claimsPrincipal = new Mock<ClaimsPrincipal>();
            claimsPrincipal.Setup(p => p.Identity.IsAuthenticated).Returns(isAuthenticated);
            _httpContext.User = claimsPrincipal.Object;
            _httpContext.RequestServices = SetupServiceProvider();
        }

        [Fact]
        public async Task OnGetAsync_ErrorMessageIsSet_AddsModelError()
        {
            // Arrange
            _loginModel.ErrorMessage = "Test error message";
            ConfigureHttpContext(false);

            // Act
            await _loginModel.OnGetAsync();

            // Assert
            Assert.True(_loginModel.ModelState.ContainsKey(string.Empty), "Expected ModelState to contain an error.");
            Assert.Equal("Test error message",
                _loginModel.ModelState[string.Empty].Errors.FirstOrDefault()?.ErrorMessage);
        }

        [Fact]
        public async Task OnGetAsync_UserAuthenticated_RedirectsToHomePage()
        {
            // Arrange
            ConfigureHttpContext(isAuthenticated: true);  // Ensure this sets the HttpContext correctly

            // Mock UserManager and SignInManager to return a valid user
            var user = new TransitNEUser(); // Create a user
            _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            // Set up SignInManager behavior
            _signInManagerMock.Setup(m => m.SignInAsync(It.IsAny<TransitNEUser>(), It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);  // Mock SignInAsync (as needed for the test)


            // Act
            await _loginModel.OnGetAsync(returnUrl: "/");  // Call OnGetAsync with a returnUrl


            // Assert: Verify the redirect happened
            Assert.True(_httpContext.Response.Headers.ContainsKey("Location"), "Expected a redirect response.");
            Assert.Equal("/", _httpContext.Response.Headers["Location"].ToString());  // Verify the redirect URL
        }

        [Fact]
        public async Task OnPostAsync_ValidCredentials_RedirectsToReturnUrl()
        {
            // Arrange
            // Mock HttpContext and Response
            var context = new Mock<HttpContext>();
            var response = new Mock<HttpResponse>();
            var headerDictionary = new HeaderDictionary();
    
            // Setting up the Headers to avoid null reference
            response.Setup(r => r.Headers).Returns(headerDictionary);
            context.Setup(c => c.User.Identity.IsAuthenticated).Returns(true);
            context.Setup(c => c.Response).Returns(response.Object);
            _loginModel.PageContext.HttpContext = context.Object;

            // Ensure SignInManager is mocked to return LockedOut result
            _signInManagerMock.Setup(m =>
                    m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);  // Mock the locked-out case

            // Mock the ModelState
            _loginModel.ModelState.AddModelError("test", "error");

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal(".~/", redirectResult.Url);
        }

        [Fact]
        public async Task OnPostAsync_TwoFactorRequired_RedirectsTo2faPage()
        {
            // Arrange
            _signInManagerMock.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./LoginWith2fa", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPostAsync_AccountLockedOut_RedirectsToLockoutPage()
        {
            // Arrange
            _signInManagerMock.Setup(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./Lockout", redirectToPageResult.PageName);
        }
    }
}
