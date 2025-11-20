using KollektivSystem.ApiService.Extensions.Endpoints;
using KollektivSystem.ApiService.Models;
using KollektivSystem.ApiService.Models.Dtos.Tickets;
using KollektivSystem.ApiService.Services.Interfaces;
using KollektivSystem.UnitTests.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KollektivSystem.UnitTests.ApiTests;

public class PurchasedTIcketEndpointsTests
{
    [Fact]
    public async Task BuyTicket_PurchaseSucceeds_ReturnsCreated()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var ticketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) },
                authenticationType: "test"));

        var request = new PurchaseTicketRequest
        {
            TicketTypeId = 1
        };

        var purchasedTicket = new PurchasedTicket
        {
            Id = ticketId,
            UserId = userId,
            TicketTypeId = request.TicketTypeId,
            ValidationCode = "12345678",
            ExpireAt = expireAt
        };

        var ticketServiceMock = new Mock<IPurchasedTicketService>();
        ticketServiceMock.Setup(s => s.PurchaseAsync(userId, request.TicketTypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(purchasedTicket);

        var logger = TestLogger.Create<PurchasedTicketEndpointsLogggerCategory>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleBuyTicket(request, ticketServiceMock.Object, user, logger.Object, CancellationToken.None);

        // Assert
        var created = Assert.IsType<Created<PurchasedTicketResponse>>(result);
        Assert.Equal($"me/{ticketId}", created.Location);

        var dto = Assert.IsType<PurchasedTicketResponse>(created.Value);
        Assert.Equal(ticketId, dto.Id);
        Assert.Equal(request.TicketTypeId, dto.TicketTypeId);
    }

    [Fact]
    public async Task BuyTicket_PurchaseFails_ReturnsProblem()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) },
                authenticationType: "test"));

        var request = new PurchaseTicketRequest
        {
            TicketTypeId = 1
        };

        var ticketServiceMock = new Mock<IPurchasedTicketService>();
        ticketServiceMock.Setup(s => s.PurchaseAsync(userId, request.TicketTypeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PurchasedTicket?)null);

        var logger = TestLogger.Create<PurchasedTicketEndpointsLogggerCategory>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleBuyTicket(request, ticketServiceMock.Object, user, logger.Object, CancellationToken.None);

        // Assert
        var statusResult = Assert.IsType<IStatusCodeHttpResult>(result, exactMatch: false);
        Assert.Equal(StatusCodes.Status409Conflict, statusResult.StatusCode);

    }

    [Fact]
    public async Task TicketValidation_ValidTicket_ReturnsValidResponse()
    {
        // Arrange
        var validationCode = "VALID123";
        var ticketId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var userId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var request = new ValidateTicketRequest { ValidationCode = validationCode };

        var ticket = new PurchasedTicket
        {
            Id = ticketId,
            UserId = userId,
            TicketTypeId = 1,
            ValidationCode = validationCode,
            ExpireAt = expireAt
        };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.ValidateAsync(validationCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, ticket, (string?)null));

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleTicketValidation(request, serviceMock.Object, loggerMock.Object, CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<ValidateTicketResponse>>(result);
        var response = ok.Value;

        Assert.NotNull(response);
        Assert.True(response.IsValid);
        Assert.Equal(ticketId, response.TicketId);
        Assert.Equal(expireAt, response.ExpireAt);
    }

    [Fact]
    public async Task TicketValidation_InvalidTicketWithoutReason_ReturnsInvalidWithDefaultReason()
    {
        // Arrange
        var validationCode = "INVALID123";

        var request = new ValidateTicketRequest
        {
            ValidationCode = validationCode
        };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.ValidateAsync(validationCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null, (string?)null));

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleTicketValidation(
            request,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<ValidateTicketResponse>>(result);

        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task TicketValidation_InvalidTicketWithReason_ReturnsInvalidWithReason()
    {
        // Arrange
        var validationCode = "INVALID123";
        var reason = "Ticket already used";

        var request = new ValidateTicketRequest { ValidationCode = validationCode };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.ValidateAsync(validationCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null, reason));

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleTicketValidation(
            request,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<ValidateTicketResponse>>(result);
        var response = ok.Value;

        Assert.NotNull(response);
        Assert.False(response.IsValid);
        Assert.Equal(reason, response.Reason);
    }

    [Fact]
    public async Task GetMe_ValidUser_ReturnsOkWithMappedTickets()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var ticketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, authenticationType: "test"));

        var includeInvalid = false;

        var ticket = new PurchasedTicket
        {
            Id = ticketId,
            UserId = userId,
            TicketTypeId = 1,
            ValidationCode = "VALID123",
            ExpireAt = expireAt
        };

        var tickets = new List<PurchasedTicket> { ticket };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetByUserAsync(userId, includeInvalid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tickets);

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleGetMe(
            includeInvalid,
            serviceMock.Object,
            user,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<IEnumerable<PurchasedTicketResponse>>>(result);
        var responses = ok.Value!.ToList();

        Assert.Single(responses);

        var dto = responses[0];
        Assert.Equal(ticketId, dto.Id);
        Assert.Equal(1, dto.TicketTypeId);
        Assert.Equal(expireAt, dto.ExpireAt);
    }

    [Fact]
    public async Task GetMe_CallsServiceWithCorrectUserIdAndIncludeInvalidFlag()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                },
                authenticationType: "test"));

        var includeInvalid = true; // choose true to make sure the flag is respected

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetByUserAsync(userId, includeInvalid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<PurchasedTicket>());

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        _ = await PurchasedTicketEndpoints.HandleGetMe(
            includeInvalid,
            serviceMock.Object,
            user,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        serviceMock.Verify(
            s => s.GetByUserAsync(userId, includeInvalid, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task GetMeById_TicketExists_ReturnsOk()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var ticketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc); 

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                },
                authenticationType: "test"));

        var ticket = new PurchasedTicket
        {
            Id = ticketId,
            UserId = userId,
            TicketTypeId = 1,
            ValidationCode = "VALID123",
            ExpireAt = expireAt
        };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetForUserByIdAsync(userId, ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleGetMeById(
            ticketId,
            serviceMock.Object,
            user,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<PurchasedTicketResponse>>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetMeById_TicketDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var ticketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                },
                authenticationType: "test"));

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetForUserByIdAsync(userId, ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PurchasedTicket?)null);

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleGetMeById(
            ticketId,
            serviceMock.Object,
            user,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result);
    }
    [Fact]
    public async Task Revoke_TicketExists_ReturnsNoContent()
    {
        // Arrange
        var ticketId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.RevokeAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // ticket exists

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleRevoke(
            ticketId,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        Assert.IsType<NoContent>(result);
    }

    [Fact]
    public async Task Revoke_TicketDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var ticketId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.RevokeAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // ticket doesn't exist

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleRevoke(
            ticketId,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result);
    }
    [Fact]
    public async Task GetById_TicketExists_ReturnsOk()
    {// Arrange
        var ticketId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var expireAt = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var ticket = new PurchasedTicket
        {
            Id = ticketId,
            UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            TicketTypeId = 42,
            ValidationCode = "VALID123",
            ExpireAt = expireAt
        };

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleGetById(
            ticketId,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        var ok = Assert.IsType<Ok<PurchasedTicketResponse>>(result);
        Assert.NotNull(ok.Value);
    }

    [Fact]
    public async Task GetById_TicketDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var ticketId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var serviceMock = new Mock<IPurchasedTicketService>();
        serviceMock
            .Setup(s => s.GetByIdAsync(ticketId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PurchasedTicket?)null);

        var loggerMock = new Mock<ILogger<PurchasedTicketEndpointsLogggerCategory>>();

        // Act
        var result = await PurchasedTicketEndpoints.HandleGetById(
            ticketId,
            serviceMock.Object,
            loggerMock.Object,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFound>(result);
    }

}
