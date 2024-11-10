using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;
using TransitNE.Areas.Identity.Pages;

namespace TransitNETesting.Tests.UnitTests
{
   public class ErrorModelTests
    {
        [Fact]
        public void OnGet_SetsRequestId_FromActivityId_WhenActivityIsNotNull()
        {
            // Arrange
            var errorModel = new ErrorModel();
            
            // Simulate Activity.Current being set with a specific Id
            var activity = new Activity("TestActivity");
            activity.Start();
            var expectedRequestId = activity.Id;

            // Act
            errorModel.OnGet();

            // Assert
            Assert.Equal(expectedRequestId, errorModel.RequestId);

            // Clean up Activity.Current
            activity.Stop();
            Activity.Current = null;
        }

        [Fact]
        public void OnGet_SetsRequestId_FromTraceIdentifier_WhenActivityIsNull()
        {
            // Arrange
            var errorModel = new ErrorModel();
            var httpContext = new DefaultHttpContext();
            var expectedRequestId = "TraceIdentifier123";
            httpContext.TraceIdentifier = expectedRequestId;

            // Setting HttpContext for the PageModel
            errorModel.PageContext = new PageContext
            {
                HttpContext = httpContext
            };

            // Act
            errorModel.OnGet();

            // Assert
            Assert.Equal(expectedRequestId, errorModel.RequestId);
        }

        [Fact]
        public void ShowRequestId_ReturnsTrue_WhenRequestIdIsNotEmpty()
        {
            // Arrange
            var errorModel = new ErrorModel
            {
                RequestId = "SomeRequestId"
            };

            // Act
            var result = errorModel.ShowRequestId;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShowRequestId_ReturnsFalse_WhenRequestIdIsEmpty()
        {
            // Arrange
            var errorModel = new ErrorModel
            {
                RequestId = ""
            };

            // Act
            var result = errorModel.ShowRequestId;

            // Assert
            Assert.False(result);
        }
    }
}

