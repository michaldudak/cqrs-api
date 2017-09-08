using System.Net.Http;
using System.Text;
using CqrsApi.Tests.TestApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace CqrsApi.Tests.Infrastructure
{
	public abstract class BaseWebTest
	{
		protected readonly HttpClient _client;
		protected string _contentRoot;

		protected BaseWebTest()
		{
			_client = GetClient();
		}

		protected HttpClient GetClient()
		{
			var builder = new WebHostBuilder().UseStartup<Startup>();
			var server = new TestServer(builder);
			var client = server.CreateClient();
			return client;
		}

		protected StringContent Json(object payload) => new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
	}
}
