using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using TransitNE.Controllers;
using TransitNE.Controllers.Interfaces;
using TransitNE.Data;
using TransitNE.Models;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

namespace TransitNETesting.Tests.UnitTests
{
public class SeptaRouteInformationControllerTests
{
    private readonly TransitNEContext _dbContext;
    private readonly Mock<ISeptaApiService> _mockApiService;
    private readonly SeptaRouteInformationController _controller;

    public SeptaRouteInformationControllerTests()
    {
        var options = new DbContextOptionsBuilder<TransitNEContext>()
            .UseInMemoryDatabase(databaseName: "TransitNETestingDb")
            .Options;

        _dbContext = new TransitNEContext(options);

        _mockApiService = new Mock<ISeptaApiService>();

        // Setup default mocked data
        _mockApiService.Setup(s => s.GetTrainDataAsync())
            .ReturnsAsync(new List<TrainModel>
            {
                new TrainModel { ID = 1, line = "Airport", trainno = "A123" },
                new TrainModel { ID = 2, line = "Airport", trainno = "A456" },
                new TrainModel { ID = 3, line = "Lansdale/Doylestown", trainno = "L789" }
            });

        _mockApiService.Setup(s => s.GetRailSchedulesAsync(It.IsAny<string>()))
            .ReturnsAsync((string trainNumber) =>
            {
                // Return a single schedule per train number for simplicity
                return new List<RailScheduleModel>
                {
                    new RailScheduleModel { ID = 1, station = "Station1", sched_tm = "10:00", est_tm = "10:05", act_tm = "10:05" }
                };
            });

        _mockApiService.Setup(s => s.GetStopsAsync(It.IsAny<string>()))
            .ReturnsAsync((string routeNumber) =>
            {
                return new List<StopModel>
                {
                    new StopModel { Id = 1, StopId = "101", StopName = "Main St" }
                };
            });

        _mockApiService.Setup(s => s.GetBusTrolleySchedulesAsync(It.IsAny<int>()))
            .ReturnsAsync((int stopId) =>
            {
                return new List<BusTrolleySchedule>
                {
                    new BusTrolleySchedule { ID = 1, Stopname = "Main St", Route = "42" }
                };
            });

        _controller = new SeptaRouteInformationController(_dbContext, _mockApiService.Object);
    }

    [Fact]
    public void SetTrainModels_Populates_Database_With_TrainData()
    {
        // Act
        _controller.SetTrainModels();

        // Assert
        var trainsInDb = _dbContext.TrainModel.ToList();
        Assert.Equal(3, trainsInDb.Count);
        Assert.Contains(trainsInDb, t => t.trainno == "A123");
    }

    [Fact]
    public void GetSeptaTrainNumbers_Returns_Correct_TrainNumbers()
    {
        // Arrange
        _controller.SetTrainModels(); // populate DB with train data

        // Act
        var result = _controller.GetSeptaTrainNumbers("Airport");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("A123", result);
        Assert.Contains("A456", result);
    }

    [Fact]
    public void GetSeptaRailSchedules_Stores_And_Returns_Schedules()
    {
        // Arrange
        _controller.SetTrainModels();
        var trainNumbers = _controller.GetSeptaTrainNumbers("Airport");

        // Act
        var schedules = _controller.GetSeptaRailSchedules(trainNumbers);

        // Assert
        Assert.NotEmpty(schedules);
        Assert.Equal(1, schedules.Count); // Each train returns one schedule in our mock setup, total 2 trains -> 2 schedules
        var schedulesInDb = _dbContext.RailScheduleModels.ToList();
        Assert.Equal(trainNumbers.Count, schedulesInDb.Count);
    }

    [Fact]
    public void GetSelectedLine_Returns_View_With_Schedules()
    {
        // Arrange
        // We need to mock form data
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "RegionalRailLine", "Airport" }
        });

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
        };

        _controller.SetTrainModels(); // Ensure DB populated

        // Act
        var result = _controller.GetSelectedLine() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as List<RailScheduleModel>;
        Assert.NotNull(model);
        Assert.NotEmpty(model);
    }

    [Fact]
    public void GetStopInformation_Retrieves_And_Stores_Stops()
    {
        // Act
        var stops = _controller.GetStopInformation("42");

        // Assert
        Assert.NotEmpty(stops);
        Assert.Contains(stops, s => s.StopName == "Main St");
        var stopsInDb = _dbContext.StopModels.ToList();
        Assert.NotEmpty(stopsInDb);
    }

    [Fact]
    public void GetBusTrolleySchedule_Retrieves_And_Stores_Schedules()
    {
        // Act
        var schedules = _controller.GetBusTrolleySchedule(101);

        // Assert
        Assert.NotEmpty(schedules);
        Assert.Contains(schedules, b => b.Route == "42");
        var schedulesInDb = _dbContext.BusTrolleySchedules.ToList();
        Assert.NotEmpty(schedulesInDb);
    }

    [Fact]
    public void GetStopId_Returns_Correct_StopId()
    {
        // Arrange
        // First populate stops
        _controller.GetStopInformation("42");

        // Act
        int stopId = _controller.GetStopId("Main St");

        // Assert
        Assert.Equal(101, stopId);
    }

    [Fact]
    public void GetSelectedBus_Returns_View_With_BusTrolleySchedules()
    {
        // Arrange
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "route", "42" },
            { "StopName", "Main St" }
        });

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { Request = { Form = formCollection } }
        };

        // Act
        var result = _controller.GetSelectedBus("42", "Main St") as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as List<BusTrolleySchedule>;
        Assert.NotNull(model);
        Assert.NotEmpty(model);
        Assert.Contains(model, m => m.Route == "42");
    }
}
}
