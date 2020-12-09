using System;
using System.Threading.Tasks;
using Xunit;

namespace LACE.Core.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task TaskPolling()
        {
            var task = Task.Delay(5000);

            var iterator = 0;
            while (!task.IsCompleted)
            {
                iterator++;
                await Task.Delay(100);
            }

            Assert.True(iterator > 45, iterator.ToString());
        }
    }
}
