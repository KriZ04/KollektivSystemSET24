using Moq;
using KollektivSystem.ApiService.Services.Implementations;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Models.Transport;
using Microsoft.Extensions.Logging;

namespace KollektivSystem.UnitTests.ApiTests
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
            Assert.NotNull(result);
            Assert.Equal(testLine, result);
            Assert.Equal("Test Line", result.Name);



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
            Assert.NotNull(result);
            Assert.Equal(testLine, result);
            Assert.Equal("Blue Line", result.Name);

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
            Assert.Null(result);

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
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equivalent(linesInRepo, result);

            repoMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        



    }
}
