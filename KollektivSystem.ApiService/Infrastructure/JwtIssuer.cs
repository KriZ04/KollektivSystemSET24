namespace KollektivSystem.ApiService.Infrastructure
{
    public class JwtIssuer : IJwtIssuer
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public JwtIssuer(string issuer, string audience, string key)
        {
            _issuer = issuer;
            _audience = audience;
            _key = key;
        }
    }
}
