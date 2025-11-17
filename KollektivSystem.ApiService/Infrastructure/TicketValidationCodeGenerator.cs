using System.Security.Cryptography;

namespace KollektivSystem.ApiService.Infrastructure
{
    public static class TicketValidationCodeGenerator
    {
        private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static string Generate(int length = 8)
        {
            var buffer = new byte[length];
            RandomNumberGenerator.Fill(buffer);

            return new string(buffer.Select(b => Chars[b % Chars.Length]).ToArray());
        }
    }
}
