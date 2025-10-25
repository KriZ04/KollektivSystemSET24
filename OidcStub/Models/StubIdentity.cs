using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Models
{
    public record StubIdentity(string Sub, string Email, string Name);
}
