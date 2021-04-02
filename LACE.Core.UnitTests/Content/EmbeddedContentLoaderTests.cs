using System.Reflection;
using LACE.Core.Content;
using LACE.Core.Extensions;
using Xunit;

namespace LACE.Core.UnitTests.Content
{
    public class EmbeddedContentLoaderTests
    {
        [Fact]
        public void LoadString()
        {
            var loader = new EmbeddedContentLoader(Assembly.GetCallingAssembly());

            var result = loader.LoadString("stub-json.json");
            Assert.False(result.IsNullOrWhiteSpace());
        }
    }
}
