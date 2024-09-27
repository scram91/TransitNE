using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using TransitNE.Data;

public class InjectFixture : IDisposable
{
    public readonly UserManager<TransitNEContext> UserManager;
    public readonly SignInManager<TransitNEContext> SignInManager;
    public readonly IAccountService AccountService;
    public readonly TransitNEContext DbContext;
    public readonly IGenerator Generator;

    public InjectFixture()
    {
        var options = new DbContextOptionsBuilder<TransitNEContext>()
            .UseInMemoryDatabase(databaseName: "FakeDatabase")
            .Options;

        DbContext = new TransitNEContext(options);

        var users = new List<TransitNEUser>
            {
                new TransitNEUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }
            }.AsQueryable();

        var fakeUserManager = new Mock<FakeUserManager>();

        fakeUserManager.Setup(x => x.Users)
            .Returns(users);

        fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<TransitNEUser>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<TransitNEUser>()))
            .ReturnsAsync(IdentityResult.Success);
        fakeUserManager.Setup(x =>
                x.ChangeEmailAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var signInManager = new Mock<FakeSignInManager>();
        signInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        UserManager = fakeUserManager.Object;
        SignInManager = signInManager.Object;
        AccountService = new AccountService(UserManager);
        Generator = new Generator();
    }

    public void Dispose()
    {
        UserManager?.Dispose();
        DbContext?.Dispose();
    }
}
