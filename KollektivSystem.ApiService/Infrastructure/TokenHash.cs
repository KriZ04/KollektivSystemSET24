namespace KollektivSystem.ApiService.Infrastructure
{
    public static class TokenHash
    {
        public static string Hash(string token)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(token);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
