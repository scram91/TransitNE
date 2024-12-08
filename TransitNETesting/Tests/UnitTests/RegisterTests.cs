using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using TransitNE.Areas.Identity.Pages.Account;
using TransitNE.Models;
using Xunit;

namespace TransitNETesting.Tests.UnitTests
{
    public class RegisterModelTests
    {
        private Mock<IUserEmailStore<TransitNEUser>> _emailStoreMock;
        private Mock<UserManager<TransitNEUser>> _userManagerMock;
        private Mock<SignInManager<TransitNEUser>> _signInManagerMock;
        private Mock<ILogger<RegisterModel>> _loggerMock;
        private Mock<IEmailSender> _emailSenderMock;

        private RegisterModel CreateModel()
        {
            _emailStoreMock = new Mock<IUserEmailStore<TransitNEUser>>();

            // Set up default behavior for the email store
            _emailStoreMock.Setup(es => es.SetEmailAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _emailStoreMock.Setup(es => es.SetUserNameAsync(It.IsAny<TransitNEUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _emailStoreMock.Setup(es => es.CreateAsync(It.IsAny<TransitNEUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock = new Mock<UserManager<TransitNEUser>>(
                _emailStoreMock.Object,
                null, null, new List<IUserValidator<TransitNEUser>>(),
                new List<IPasswordValidator<TransitNEUser>>(),
                null, null, null, null
            );

            _signInManagerMock = new Mock<SignInManager<TransitNEUser>>(
                _userManagerMock.Object,
                new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TransitNEUser>>().Object,
                null, null, null, null
            );

            _loggerMock = new Mock<ILogger<RegisterModel>>();
            _emailSenderMock = new Mock<IEmailSender>();

            var model = new RegisterModel(
                _userManagerMock.Object,
                _emailStoreMock.Object,
                _signInManagerMock.Object,
                _loggerMock.Object,
                _emailSenderMock.Object
            )
            {
                PageContext = new PageContext(),
                Url = new Mock<IUrlHelper>().Object
            };

            return model;
        }

        [Fact]
        public async Task OnGetAsync_SetsExternalLogins()
        {
            var model = CreateModel();
            _signInManagerMock.Setup(sm => sm.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>
                {
                    new AuthenticationScheme("TestScheme", "TestDisplay", typeof(object))
                });

            await model.OnGetAsync("/returnUrl");

            Assert.Equal("/returnUrl", model.ReturnUrl);
            Assert.Single(model.ExternalLogins);
            Assert.Equal("TestScheme", model.ExternalLogins[0].Name);
        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalid_ReturnsPage()
        {
            var model = CreateModel();
            model.ModelState.AddModelError("Email", "Required");
            model.Input = new RegisterModel.InputModel
            {
                Email = "notanemail",
                Password = "password",
                ConfirmPassword = "password"
            };

            var result = await model.OnPostAsync("/returnUrl");

            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }

        [Fact]
        public async Task OnPostAsync_UserCreationFails_ReturnsPageWithErrors()
        {
            var model = CreateModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            // Mocking a failed CreateAsync call
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), "Password123!"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Creation Error" }));

            var result = await model.OnPostAsync("/returnUrl");

            var pageResult = Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
            Assert.Contains(model.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                m => m.Contains("Creation Error"));
        }

        [Fact]
        public async Task OnPostAsync_SuccessfulCreation_RequireConfirmedAccount_False_LogsUserIn()
        {
            var model = CreateModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), "Password123!"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.Options.SignIn.RequireConfirmedAccount).Returns(false);
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<TransitNEUser>()))
                .ReturnsAsync("TestToken");

            _emailSenderMock.Setup(e => e.SendEmailAsync("test@example.com", "Confirm your email", It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _signInManagerMock.Setup(sm => sm.SignInAsync(It.IsAny<TransitNEUser>(), false, null))
                .Returns(Task.CompletedTask);

            var result = await model.OnPostAsync("/home");

            var redirectResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal("/home", redirectResult.Url);
            _signInManagerMock.Verify(sm => sm.SignInAsync(It.IsAny<TransitNEUser>(), false, null), Times.Once);
        }

        [Fact]
        public async Task OnPostAsync_SuccessfulCreation_RequireConfirmedAccount_True_RedirectsToConfirmation()
        {
            var model = CreateModel();
            model.Input = new RegisterModel.InputModel
            {
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "Jane",
                LastName = "Doe"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<TransitNEUser>(), "Password123!"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.Options.SignIn.RequireConfirmedAccount).Returns(true);
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<TransitNEUser>()))
                .ReturnsAsync("ConfirmationToken");

            _emailSenderMock.Setup(e => e.SendEmailAsync("test@example.com", "Confirm your email", It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var result = await model.OnPostAsync("/returnUrl");

            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("RegisterConfirmation", redirectResult.PageName);
            Assert.Equal("test@example.com", redirectResult.RouteValues["email"]);
            Assert.Equal("/returnUrl", redirectResult.RouteValues["returnUrl"]);
        }
    }
}
