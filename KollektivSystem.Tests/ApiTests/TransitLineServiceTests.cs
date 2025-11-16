using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Repositories;
using KollektivSystem.ApiService.Repositories.Uow;
using KollektivSystem.ApiService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace KollektivSystem.UnitTests.ApiTests
{
    public class TransitLineServiceTests    {
        [Fact]
        public async Task CreateAsync_WithTransitLine_ReturnsNewTransitLine()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            ILogger<TransitLineService> logger = NullLogger<TransitLineService>.Instance;

            uowMock.SetupGet(u => u.TransitLines).Returns(repoMock.Object);

            var testLine = new TransitLine
            {
                Id = 1,
                Name = "Test Line",
                Stops = new List<Stop>()
            };

            repoMock.Setup(r => r.AddAsync(It.IsAny<TransitLine>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new TransitLineService(uowMock.Object, logger);

            // Act
            var result = await service.CreateAsync(testLine);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(testLine, result);
            Assert.Equal("Test Line", result.Name);



            repoMock.Verify(r => r.AddAsync(It.IsAny<TransitLine>(), It.IsAny<CancellationToken>()), Times.Once);
            uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingLine_ReturnsTransitLine()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            ILogger<TransitLineService> logger = NullLogger<TransitLineService>.Instance;

            uowMock.SetupGet(u => u.TransitLines).Returns(repoMock.Object);

            var testLine = new TransitLine
            {
                Id = 1,
                Name = "Blue Line",
                Stops = new List<Stop>()
            };
            repoMock.Setup(r => r.FindAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(testLine);

            uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new TransitLineService(uowMock.Object, logger);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testLine, result);
            Assert.Equal("Blue Line", result.Name);

            repoMock.Verify(r => r.FindAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingLine_ReturnsNull()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            ILogger<TransitLineService> logger = NullLogger<TransitLineService>.Instance;

            uowMock.SetupGet(u => u.TransitLines).Returns(repoMock.Object);

            repoMock.Setup(r => r.FindAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TransitLine?)null);

            uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new TransitLineService(uowMock.Object, logger);

            // Act
            var result = await service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);

            repoMock.Verify(r => r.FindAsync(999, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepoReturnsLines_ReturnsThoseLines()
        {
            // Arrange
            var repoMock = new Mock<ITransitLineRepository>();
            var uowMock = new Mock<IUnitOfWork>();
            ILogger<TransitLineService> logger = NullLogger<TransitLineService>.Instance;

            uowMock.SetupGet(u => u.TransitLines).Returns(repoMock.Object);

            var linesInRepo = new List<TransitLine>
            {
                new TransitLine { Id = 1, Name = "Line 1", Stops = new List<Stop>() },
                new TransitLine { Id = 2, Name = "Line 2", Stops = new List<Stop>() }
            };

            // Repository mock
            repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(linesInRepo);

            uowMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var service = new TransitLineService(uowMock.Object, logger);

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
