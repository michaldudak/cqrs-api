using System;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi.Web.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCqrsApi(this IMvcBuilder mvcBuilder,
			Action<CqrsApiOptions> configurationAction)
		{
			mvcBuilder.ConfigureApplicationPartManager(p =>
			{
				p.FeatureProviders.Add(new CommandControllerFeatureProvider());
				p.FeatureProviders.Add(new QueryControllerFeatureProvider());
			});
		}
	}
}