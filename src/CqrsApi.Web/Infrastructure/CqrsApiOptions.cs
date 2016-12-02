using System.Collections.Generic;

namespace CqrsApi.Web.Infrastructure
{
	public class CqrsApiOptions
	{
		private readonly List<CqrsApiRouteConfiguration> _configs = new List<CqrsApiRouteConfiguration>();

		public CqrsApiRouteConfiguration MapGet(string urlTemplate)
		{
			return Map("GET", urlTemplate);
		}

		public CqrsApiRouteConfiguration MapPost(string urlTemplate)
		{
			return Map("POST", urlTemplate);
		}

		public CqrsApiRouteConfiguration Map(string verb, string urlTemplate)
		{
			var routeConfig = new CqrsApiRouteConfiguration(verb, urlTemplate);
			_configs.Add(routeConfig);
			return routeConfig;
		}
	}
}