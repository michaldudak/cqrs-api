using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi
{
	public static class ServiceCollectionExtensions
	{
		public static IMvcBuilder AddMvcWithCqrsApi(this IServiceCollection services, Action<MvcOptions> mvcSetupAction = null, Action<CqrsApiOptions> cqrsApiSetupAction = null)
		{
			var mvcBuilder = services.AddMvc(config => {
				mvcSetupAction?.Invoke(config);
				config.Conventions.Add(new CqrsApiControllerMetadataConvention());
			});

			SetupCqrsApi(mvcBuilder, cqrsApiSetupAction);

			return mvcBuilder;
		}
		
		private static void SetupCqrsApi(IMvcBuilder mvcBuilder, Action<CqrsApiOptions> configurationAction)
		{
			var cqrsApiOptions = new CqrsApiOptions();
			// TODO: come up with sensible defaults if nothing is provided.
			configurationAction?.Invoke(cqrsApiOptions);

			mvcBuilder.ConfigureApplicationPartManager(p =>
			{
				p.FeatureProviders.Add(new CommandControllerFeatureProvider(cqrsApiOptions));
				p.FeatureProviders.Add(new QueryControllerFeatureProvider(cqrsApiOptions));
			});

			mvcBuilder.Services.AddSingleton(cqrsApiOptions);
		}
	}
}