using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epinova.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Epinova.InfrastructureTests
{
    public class AsyncHelperTests
    {
        private readonly ITestOutputHelper _output;

        public AsyncHelperTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void RunSync_TaskWithReturnValue_Works()
        {
            string result = AsyncHelper.RunSync(() => StringFetcherTask("hello, there"));

            _output.WriteLine("Result: " + result);

            Assert.Equal("You said 'hello, there'", result);
        }

        [Fact]
        public void RunSync_VoidTask_Works()
        {
            var input = new[] { "hello, there" };

            AsyncHelper.RunSync(() => VoidTask(input));

            _output.WriteLine("Result: " + input);

            Assert.Equal("Modified: 'hello, there'", input.First());
        }

        private static async Task<string> StringFetcherTask(string input)
        {
            return await Task.FromResult($"You said '{input}'");
        }

        private static async Task VoidTask(IList<string> input)
        {
            await Task.Run(() => { input[0] = $"Modified: '{input[0]}'"; });
        }
    }
}
