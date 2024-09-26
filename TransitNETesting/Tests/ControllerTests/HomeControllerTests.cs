using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransitNE.Controllers;

namespace TransitNETesting.Tests.ControllerTests;
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void TestHomeIndexView()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestHomePrivacyView()
        {
            var controller = new HomeController();
            var result = controller.Privacy() as ViewResult;
            Assert.AreEqual("Privacy", result.ViewName);
        }
    }
}