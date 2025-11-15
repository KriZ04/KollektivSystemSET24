using Xunit;
using Moq;
using FluentAssertions;
using KollektivSystem.ApiService.Services.Implementations;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Models.Transport;
using Microsoft.Extensions.Logging;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KollektivSystem.UnitTests.ApiTest
{
    public class TransitLineServiceTests    {
        [Fact]
        public async Task CreateAsync_WithTransitLine_ReturnsNewTransitLine()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var loggerMock = new Mock<ILogger<TransitLineService>>();

            var testLine = new TransitLine
            {
                Id = 1,
                Name = "Test Line",
                Stops = new List<Stop>()
            };

            repoMock.Setup(r => r.AddAsync(It.IsAny<TransitLine>(), default))
                    .Returns(Task.CompletedTask);

            repoMock.Setup(r => r.SaveChanges())
                    .Returns(Task.CompletedTask);

            var service = new TransitLineService(repoMock.Object, loggerMock.Object);

            // Act
            var result = await service.CreateAsync(testLine);


            // Assert
            result.Should().NotBeNull();
            result.Should().Be(testLine);
            result.Name.Should().Be("Test Line");

            repoMock.Verify(r => r.AddAsync(It.IsAny<TransitLine>(), default), Times.Once);
            repoMock.Verify(r => r.SaveChanges(), Times.Once);



        }

        [Fact]
        public async Task GetByIdAsync_WithExistingLine_ReturnsTransitLine()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var loggerMock = new Mock<ILogger<TransitLineService>>();

            var testLine = new TransitLine
            {
                Id = 1,
                Name = "Blue Line",
                Stops = new List<Stop>()
            };

            repoMock
                .Setup(r => r.FindAsync(1, default))
                .ReturnsAsync(testLine);

            var service = new TransitLineService(repoMock.Object, loggerMock.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(testLine);
            result!.Name.Should().Be("Blue Line");

            repoMock.Verify(r => r.FindAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingLine_ReturnsNull()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var loggerMock = new Mock<ILogger<TransitLineService>>();

            repoMock
                .Setup(r => r.FindAsync(999, default))
                .ReturnsAsync((TransitLine?)null);

            var service = new TransitLineService(repoMock.Object, loggerMock.Object);

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();

            repoMock.Verify(r => r.FindAsync(999, default), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepoReturnsLines_ReturnsThoseLines()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var loggerMock = new Mock<ILogger<TransitLineService>>();

            var linesInRepo = new List<TransitLine>
    {
        new TransitLine { Id = 1, Name = "Line 1", Stops = new List<Stop>() },
        new TransitLine { Id = 2, Name = "Line 2", Stops = new List<Stop>() }
    };

            // Repository mock
            repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(linesInRepo);

            var service = new TransitLineService(repoMock.Object, loggerMock.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(linesInRepo);

            repoMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        



    }
}
