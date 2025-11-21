using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Exceptions
{
    public sealed class OidcException : Exception
    {
        public OidcException(string error) : base(error) { Error = error; }
        public string Error { get; }
    }
}
