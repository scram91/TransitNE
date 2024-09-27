using Fluent.Infrastructure.FluentModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TransitNE.Data;

public class FakeSignInManager : SignInManager<TransitNEUser>
{
    public FakeSignInManager()
        : base(new Mock<FakeUserManager>().Object,
            new HttpContextAccessor(),
            new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<TransitNEUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object)
    {
    }
}