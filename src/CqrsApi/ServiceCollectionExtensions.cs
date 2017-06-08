using System;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCqrsApi(this IMvcBuilder mvcBuilder,
			Action<CqrsApiOptions> configurationAction)
		{
			var cqrsApiOptions = new CqrsApiOptions();
			configurationAction(cqrsApiOptions);

			mvcBuilder.ConfigureApplicationPartManager(p =>
			{
				p.FeatureProviders.Add(new CommandControllerFeatureProvider(cqrsApiOptions));
				p.FeatureProviders.Add(new QueryControllerFeatureProvider(cqrsApiOptions));
			});

			mvcBuilder.Services.AddSingleton(cqrsApiOptions);
		}
	}
}