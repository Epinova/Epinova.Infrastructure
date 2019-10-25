using System;
using System.Collections.Generic;
using EPiServer.Logging;
using Xunit.Abstractions;

namespace Epinova.InfrastructureTests.Logging
{
    internal class TestableLogger : ILogger
    {
        private readonly Level _minimum;
        private readonly ITestOutputHelper _output;

        public TestableLogger(Level minimum, ITestOutputHelper output)
        {
            _minimum = minimum;
            _output = output;
        }

        public List<string> Messages { get; set; } = new List<string>();

        public bool IsEnabled(Level level)
        {
            return level >= _minimum;
        }

        public void Log<TState, TException>(Level level, TState state, TException exception, Func<TState, TException, string> messageFormatter, Type boundaryType) where TException : Exception
        {
            if (IsEnabled(level))
            {
                string message = $"{level.ToString().ToUpper()}: {messageFormatter(state, exception)}";
                _output.WriteLine(message);
                Messages.Add(message);
            }
        }
    }
}
