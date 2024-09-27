using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TransitNETesting.Tests;
using System.Net;
using Fluent.Infrastructure.FluentModel;
using TransitNE.Data;
namespace TransitNETesting.Tests.IntegrationTests;

public class AuthTests 
{
    private readonly InjectFixture _injectFixture;

    [Theory]
    [InlineData("test@test.it", "Bred", "Apollo")]
    public async Task ConfirmEmail_ShouldThrowException_IfUserIdIsNullAsync(string email, string firstName,
    string lastName)
    {
        var user = new TransitNEUser()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            //This will be implemented later
            //PasswordExpiredDate = _injectFixture.Generator.UtcNow().AddDays(passwordExpireInDays),
        };

        Task Act() => _injectFixture.AccountService.ConfirmEmailAsync(null, user.Email, "test");

        await Assert.ThrowsAsync<ArgumentNullException>(Act);

        Assert.Contains("userId", Act().Exception.Message);
    }

    [Theory]
    [InlineData("test@test.it", "Bred", "Apollo")]
    public async Task ConfirmEmail_ShouldThrowException_IfCodeIsNullAsync(string email, string firstName,
    string lastName /*int passwordExpireInDays*/)
    {
        var user = new TransitNEUser()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            //PasswordExpiredDate = _injectFixture.Generator.UtcNow().AddDays(passwordExpireInDays),
        };

        Task Act() => _injectFixture.AccountService.ConfirmEmailAsync(user.Id, user.Email, null);

        await Assert.ThrowsAsync<ArgumentNullException>(Act);

        Assert.Contains("code", Act().Exception.Message);
    }
    [Theory]
    [InlineData("test@test.it", "Bred", "Apollo"/*, 90*/, "wertyuiolkjmnbvcfdew3456789")]
    public async Task ConfirmEmail_ShouldThrowException_IfUserIsNullAsync(string email, string firstName,
    string lastName/*, int passwordExpireInDays*/, string code)
    {
        var user = new TransitNEUser()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            //PasswordExpiredDate = _injectFixture.Generator.UtcNow().AddDays(passwordExpireInDays),
        };

        Task Act() => _injectFixture.AccountService.ConfirmEmailAsync(user.Id, user.Email, code);

        await Assert.ThrowsAsync<ArgumentNullException>(Act);

        Assert.Contains("user", Act().Exception.Message);
    }


}