using Microsoft.AspNetCore.Http;
using OidcStub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Services
{
    public interface IOidcTokenService
    {
        TokenResponse ExchangeCode(IFormCollection form, CancellationToken ct);
    }
}
