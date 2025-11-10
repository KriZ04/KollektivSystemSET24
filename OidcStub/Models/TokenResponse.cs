using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Models
{
    public sealed record TokenResponse(string IdToken, string AccessToken, string TokenType, int ExpiresIn);
}
