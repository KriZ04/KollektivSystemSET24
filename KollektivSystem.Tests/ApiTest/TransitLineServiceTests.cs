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

    }
}
