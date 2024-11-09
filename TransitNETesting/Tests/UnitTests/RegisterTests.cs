using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Data;
using TransitNE.Models;

namespace TransitNETesting.Tests.UnitTests
{
    public class RegisterModelTests
    {
        private readonly Mock<UserManager<TransitNEUser>> _userManagerMock;
        private readonly Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private readonly RegisterModel _registerModel;
        private readonly DefaultHttpContext _httpContext;


        public RegisterModelTests()
        {
            _httpContext = new DefaultHttpContext();

            _userManagerMock = MockUserManager();
            _signInManagerMock = MockSignInManager();

            _registerModel = new RegisterModel(_userManagerMock.Object, new UserStore<TransitNEUser>(null, null), _signInManagerMock.Object,
                                    new LoggerFactory().CreateLogger<RegisterModel>(), new NoOpEmailSender())
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
    }
}
