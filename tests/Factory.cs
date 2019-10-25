using System;

namespace Epinova.InfrastructureTests
{
    internal class Factory
    {
        /// <summary>
        /// Gets a positive integer.
        /// </summary>
        /// <returns></returns>
        public static int GetInteger()
        {
            return Math.Abs(GetString().GetHashCode());
        }

        public static string GetString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
