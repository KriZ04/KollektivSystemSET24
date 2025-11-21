using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KollektivSystem.UnitTests.Setup
{
    internal static class TestLogger
    {
        internal static Mock<ILogger<T>> Create<T>() => new Mock<ILogger<T>>();
    }
}
