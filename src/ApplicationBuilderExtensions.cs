using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CqrsApi
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseMvcWithCqrsApi(this IApplicationBuilder applicationBuilder, Action<IRouteBuilder> configureRoutes = null)
		{
			var apiOptions = applicationBuilder.ApplicationServices.GetService(typeof (CqrsApiOptions)) as CqrsApiOptions;

			if (apiOptions == null)
			{
				throw new ArgumentNullException(nameof(apiOptions));
			}

			applicationBuilder.UseMvc(routes =>
			{
				apiOptions.PopulateRoutes(routes);
				configureRoutes?.Invoke(routes);
			});

			return applicationBuilder;
		}
	}
}