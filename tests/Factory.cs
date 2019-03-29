using System;

namespace Epinova.InfrastructureTests
{
    internal class Factory
    {
        public static string GetString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}