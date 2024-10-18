using TransitNE.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using TransitNETesting.Utilities;

namespace TransitNETesting.Tests.UnitTests
{
    public class SeptaRouteInformationTests : IClassFixture<TestDatabaseFixture>, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public SeptaRouteInformationTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
            var factory = new WebApplicationFactory<Program>();
            _factory = factory;
        }

        public TestDatabaseFixture Fixture { get; }

        [Fact]
        public void GetTrainInformationTest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();
            var controller = new SeptaRouteInformationController(context);
            //Act
            controller.SetTrainModels();
            context.ChangeTracker.Clear();
            var response = context.TrainModel.Single(a => a.ID == 1);

            //Assert
            Assert.Equal("1717", response.trainno);
        }
        [Fact]
        public void GetSeptaTrainNumberTest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();
            var controller = new SeptaRouteInformationController(context);

            //Act
            var line = controller.GetSeptaTrainNumbers("Trenton");
            context.ChangeTracker.Clear();
            //Assert
            Assert.Equal("1717", line[0]);
        }

        [Fact]
        public void GetStopIdTest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            var controller = new SeptaRouteInformationController(context);
            //Act
            var stopId = controller.GetStopId("Street Rd & Kingston Way");

            //Assert
            Assert.Equal(13520, stopId);
        }

        [Fact]
        public void GetStopInformationTest()
        {
            //Arrange
            using var context = Fixture.CreateContext();
            context.Database.BeginTransaction();
            var controller = new SeptaRouteInformationController(context);

            //Act
            var stopInfo = controller.GetStopInformation("1");
            context.ChangeTracker.Clear();
            //Assert
            Assert.NotEmpty(stopInfo);
        }
    }
}
