using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Stops;
using KollektivSystem.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace KollektivSystem.UnitTests.ApiTests;

public class StopEndpointsTests
{
    [Fact]
    public async Task GetAll_WhenServiceReturnsNull_ReturnsNotFound()
    {
        // Arrange
        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync((IReadOnlyList<Stop>?)null);

        // Act
        var result = await StopEndpoints.HandleGetAll(service.Object, CancellationToken.None);

        // Assert
        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status404NotFound, status.StatusCode);

        Assert.IsType<NotFound>(result);

        service.VerifyAll();
    }

    [Fact]
    public async Task GetAll_WhenStopsReturned_ReturnsOkWithMappedStops()
    {
        // Arrange
        var stops = new List<Stop>
        {
            new Stop { Id = 1, Name = "Stop A" },
            new Stop { Id = 2, Name = "Stop B" }
        };

        IReadOnlyList<Stop> readOnly = stops;

        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(readOnly);

        // Act
        var result = await StopEndpoints.HandleGetAll(service.Object, CancellationToken.None);

        // Assert
        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status200OK, status.StatusCode);

        var value = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(value.Value);

        var list = Assert.IsAssignableFrom<IEnumerable<object>>(value.Value);
        Assert.Equal(2, list.Count());

        service.VerifyAll();
    }

    [Fact]
    public async Task GetById_WhenStopNotFound_ReturnsNotFound()
    {
        var service = new Mock<IStopService>(MockBehavior.Strict);

        service.Setup(s => s.GetByIdAsync(10, It.IsAny<CancellationToken>()))
               .ReturnsAsync((Stop?)null);

        var result = await StopEndpoints.HandleGetStopById(10, service.Object, CancellationToken.None);

        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status404NotFound, status.StatusCode);

        Assert.IsType<NotFound>(result);

        service.VerifyAll();
    }

    [Fact]
    public async Task GetById_WhenStopFound_ReturnsOk()
    {
        var stop = new Stop { Id = 5, Name = "Central" };

        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.GetByIdAsync(5, It.IsAny<CancellationToken>()))
               .ReturnsAsync(stop);

        var result = await StopEndpoints.HandleGetStopById(5, service.Object, CancellationToken.None);

        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status200OK, status.StatusCode);

        var value = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.NotNull(value.Value);

        service.VerifyAll();
    }

    [Fact]
    public async Task Create_WhenServiceReturnsNull_ReturnsProblem()
    {
        var req = new CreateStopRequest
        {
            Name = "New Stop",
            Latitude = 59.123,
            Longitude = 10.456
        };

        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.CreateAsync(req, It.IsAny<CancellationToken>()))
               .ReturnsAsync((Stop?)null);

        var result = await StopEndpoints.HandleCreateStop(req, service.Object, CancellationToken.None);

        Assert.IsType<ProblemHttpResult>(result);

        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status500InternalServerError, status.StatusCode);

        service.VerifyAll();
    }

    [Fact]
    public async Task Create_WhenStopCreated_ReturnsCreated()
    {
        var req = new CreateStopRequest
        {
            Name = "Station X",
            Latitude = 58.12,
            Longitude = 11.67
        };

        var createdStop = new Stop { Id = 123, Name = "Station X" };

        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.CreateAsync(req, It.IsAny<CancellationToken>()))
               .ReturnsAsync(createdStop);

        var result = await StopEndpoints.HandleCreateStop(req, service.Object, CancellationToken.None);

        var created = Assert.IsType<Created<StopResponse>>(result);
        Assert.Equal("123", created.Location);
        Assert.NotNull(created.Value);

        service.VerifyAll();
    }

    [Fact]
    public async Task Delete_WhenDeleteFails_ReturnsNotFound()
    {
        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.DeleteAsync(15, It.IsAny<CancellationToken>()))
               .ReturnsAsync(false);

        var result = await StopEndpoints.HandleDeleteStop(15, service.Object, CancellationToken.None);

        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status404NotFound, status.StatusCode);

        Assert.IsType<NotFound>(result);

        service.VerifyAll();
    }

    [Fact]
    public async Task Delete_WhenSuccess_ReturnsOk()
    {
        var service = new Mock<IStopService>(MockBehavior.Strict);
        service.Setup(s => s.DeleteAsync(9, It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

        var result = await StopEndpoints.HandleDeleteStop(9, service.Object, CancellationToken.None);

        var status = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status200OK, status.StatusCode);

        var value = Assert.IsType<IValueHttpResult>(result, exactMatch: false);
        Assert.Contains("9", value.Value!.ToString());

        service.VerifyAll();
    }
}
