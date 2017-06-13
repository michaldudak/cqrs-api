using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace CqrsApi
{
	public class CqrsApiOptions
	{
		internal List<CqrsApiRouteConfiguration> Configs { get; } = new List<CqrsApiRouteConfiguration>();

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
			Configs.Add(routeConfig);
			return routeConfig;
		}

		internal void PopulateRoutes(IRouteBuilder routeBuilder)
		{
			foreach (var config in Configs)
			{
				routeBuilder.MapRoute(
					config.InputType.FullName,
					config.UrlTemplate,
					new {controller = config.InputType.Name, action = "Index"},
					new RouteValueDictionary(new {httpMethod = new HttpMethodRouteConstraint(config.Verb)}));
			}
		}
	}
}