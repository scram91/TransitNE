using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using TransitNE.Models;

namespace TransitNETesting.Utilities.Helpers
{

public static class IdentityTestHelpers
{
public static UserManager<TransitNEUser> GetTestUserManager()
{
    var store = new Mock<IUserStore<TransitNEUser>>();
    var options = new Mock<IOptions<IdentityOptions>>();
    var passwordHasher = new Mock<IPasswordHasher<TransitNEUser>>();
    var userValidators = new List<IUserValidator<TransitNEUser>>();
    var passwordValidators = new List<IPasswordValidator<TransitNEUser>>();
    var keyNormalizer = new Mock<ILookupNormalizer>();
    var errors = new IdentityErrorDescriber();
    var services = new Mock<IServiceProvider>();
    var logger = new Mock<ILogger<UserManager<TransitNEUser>>>();

    return new UserManager<TransitNEUser>(
        store.Object,
        options.Object,
        passwordHasher.Object,
        userValidators,
        passwordValidators,
        keyNormalizer.Object,
        errors,
        services.Object,
        logger.Object
    );
}

public static SignInManager<TransitNEUser> GetTestSignInManager(UserManager<TransitNEUser> userManager)
{
    var contextAccessor = new Mock<IHttpContextAccessor>();
    var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>();
    var options = new Mock<IOptions<IdentityOptions>>();
    var logger = new Mock<ILogger<SignInManager<TransitNEUser>>>();
    var schemes = new Mock<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>();
    var confirmation = new Mock<IUserConfirmation<TransitNEUser>>();

    return new SignInManager<TransitNEUser>(
        userManager,
        contextAccessor.Object,
        claimsFactory.Object,
        options.Object,
        logger.Object,
        schemes.Object,
        confirmation.Object
    );
}
}
}
