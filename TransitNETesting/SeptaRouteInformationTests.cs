using System.Web.Mvc;
using TransitNE.Controllers;
using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TransitNETesting
{
    public class SeptaRouteInformationTests : IClassFixture<TestDatabaseFixture>, IClassFixture<WebApplicationFactory<Program>>
    {
        public SeptaRouteInformationTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
        }

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public async Task GetSelectedBusTest()
        {
            //Arrange
            using var _context = Fixture.CreateContext();
            var controller = new SeptaRouteInformationController(_context);
            //Act
            ViewResult busData = (ViewResult)controller.GetSelectedBus("1", "Street Rd & Kingston Way");
            //Asert
            Assert.NotNull(busData);
        }
        [Fact]
        public void GetTrainInformation()
        {
            using var context = Fixture.CreateContext();
            //context.Database.BeginTransaction();

            var controller = new SeptaRouteInformationController(context);
            controller.SetTrainModels();

            //context.ChangeTracker.Clear();

            var response = context.TrainModel.Single(a => a.ID == 1);
            Assert.Equal("1717", response.trainno);
        }
        [Fact]
        public async Task GetSelectedLineTest()
        {
            //Arange
            using var _context = Fixture.CreateContext();
            var controller = new SeptaRouteInformationController(_context);
            //Act
            var response = controller.GetSelectedLine();
            //Assert
            Assert.NotNull(response);
        }
    }
}
