using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xunit;
using TransitNE.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace TransitNETesting.Tests.UnitTests
{
public class ManageNavPagesTests
{
    private ViewContext CreateViewContext(string activePage)
    {
        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            ["ActivePage"] = activePage
        };

        var actionDescriptor = new ActionDescriptor
        {
            DisplayName = activePage
        };

        return new ViewContext
        {
            ViewData = viewData,
            ActionDescriptor = actionDescriptor
        };
    }

    [Theory]
    [InlineData("Index", "Index", "active")]
    [InlineData("Index", "Email", null)]
    [InlineData("Email", "Email", "active")]
    [InlineData("Email", "ChangePassword", null)]
    [InlineData("ChangePassword", "ChangePassword", "active")]
    [InlineData("ChangePassword", "Index", null)]
    public void PageNavClass_ReturnsExpected(string activePage, string testPage, string expected)
    {
        // Arrange
        var viewContext = CreateViewContext(activePage);

        // Act
        var result = ManageNavPages.PageNavClass(viewContext, testPage);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Index", "active")]
    [InlineData("Email", null)]
    public void IndexNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.IndexNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Email", "active")]
    [InlineData("Index", null)]
    public void EmailNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.EmailNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ChangePassword", "active")]
    [InlineData("Email", null)]
    public void ChangePasswordNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.ChangePasswordNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("DownloadPersonalData", "active")]
    [InlineData("Index", null)]
    public void DownloadPersonalDataNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.DownloadPersonalDataNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("DeletePersonalData", "active")]
    [InlineData("Email", null)]
    public void DeletePersonalDataNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.DeletePersonalDataNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ExternalLogins", "active")]
    [InlineData("ChangePassword", null)]
    public void ExternalLoginsNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.ExternalLoginsNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("PersonalData", "active")]
    [InlineData("ExternalLogins", null)]
    public void PersonalDataNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.PersonalDataNavClass(viewContext);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("TwoFactorAuthentication", "active")]
    [InlineData("DeletePersonalData", null)]
    public void TwoFactorAuthenticationNavClass_ReturnsExpected(string activePage, string expected)
    {
        var viewContext = CreateViewContext(activePage);
        var result = ManageNavPages.TwoFactorAuthenticationNavClass(viewContext);
        Assert.Equal(expected, result);
    }
}
}

