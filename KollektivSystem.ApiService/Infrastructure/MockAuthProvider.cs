using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KollektivSystem.ApiService.Infrastructure
{
    public class MockAuthProvider : IAuthProvider
    {
        private readonly string _issuer;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly SecurityKey _signingKey;

        public MockAuthProvider(string issuer, string clientId, string clientSecret, string signingKey)
        {
            _issuer = issuer;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }
    }
}
