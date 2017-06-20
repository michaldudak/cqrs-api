using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;
using CqrsApi.Tests.TestApp;
using System.Net;

namespace CqrsApi.Tests.Infrastructure
{
	public abstract class BaseWebTest
	{
		protected readonly HttpClient _client;
		protected string _contentRoot;
		public BaseWebTest()
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

		protected StringContent Json(object payload) => new StringContent(JsonConvert.SerializeObject(payload), UTF8Encoding.UTF8, "application/json");
	}
}
