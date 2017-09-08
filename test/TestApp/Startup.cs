using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CqrsEssentials.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi.Tests.TestApp
{
	public class Startup
	{
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvcWithCqrsApi(cqrsApiSetupAction: options =>
					{
						options.Builder
							.AddQuery<GetNumbersQuery>()
							.Build();
					});

			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsImplementedInterfaces();
			builder.RegisterModule<CqrsEssentialsAutofacModule>();
			builder.Populate(services);
			var container = builder.Build();

			return new AutofacServiceProvider(container);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseMvc();
		}
	}
}
