using System.Net;
using System.Threading.Tasks;
using CqrsApi.Tests.Infrastructure;
using CqrsApi.Tests.TestApp;
using Newtonsoft.Json;
using Xunit;

namespace CqrsApi.Tests
{
	public class QueryExecutionTests : BaseWebTest
	{
		[Fact]
		public async Task GivenQueryRegisteredWithDefaultRoute_WhenCalledItsApi_ShouldReturnCorrectValues()
		{
			var client = GetClient();
			var response = await client.GetAsync("GetNumbers?count=5");
			var payload = JsonConvert.DeserializeObject<int[]>(await response.Content.ReadAsStringAsync());

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(new[] { 1, 2, 3, 4, 5 }, payload);
		}
	}
}
