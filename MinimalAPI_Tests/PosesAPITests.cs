using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MinimalAPI_Tests
{
    public class PosesAPITests
    {
        [Fact]
        public async Task TestRootEndpoint()
        {
            //.Net 6 WAF is the best option for minimal API 'unit tests' - .Net 7 makes IResult available
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var response = await client.GetStringAsync("/");

            Assert.Equal("Hello from the Poses minimal API!", response);
        }
    }
}