using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OidcStub.Entities
{
    public class Persona
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string Sub { get; set; } = null!;
        public string Name { get; set; } = null!; 
        public string? Email { get; set; }
    }
}
