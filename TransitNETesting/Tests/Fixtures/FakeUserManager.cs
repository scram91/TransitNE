using Fluent.Infrastructure.FluentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TransitNE.Data;

public class FakeUserManager : UserManager<TransitNEUser>
{
    public FakeUserManager()
        : base(new Mock<IUserStore<TransitNEUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<TransitNEUser>>().Object,
            new IUserValidator<TransitNEUser>[0],
            new IPasswordValidator<TransitNEUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<TransitNEUser>>>().Object)
    {
    }
}