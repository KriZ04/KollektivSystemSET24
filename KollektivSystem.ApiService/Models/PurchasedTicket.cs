using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace KollektivSystem.ApiService.Models
{
    public class PurchasedTicket
    {
        public Guid Id {  get; init; }
        public required int TicketTypeId { get; init; }
        public TicketType TicketType { get; init; } = null!;
        public required Guid UserId { get; init; }
        public User User { get; init; } = null!;
        [StringLength(8)]
        public required string ValidationCode { get; init; }
        public DateTimeOffset PurchasedAt { get; init; } = DateTimeOffset.UtcNow;
        public required DateTimeOffset ExpireAt { get; init; }
        public bool Revoked { get; set; } = false;
        public bool IsExpired => DateTimeOffset.UtcNow > ExpireAt;
        public bool IsValid => !Revoked && !IsExpired;
    }
}
