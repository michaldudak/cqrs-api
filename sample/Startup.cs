using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CqrsApi.Sample.CommandsAndQueries;
using CqrsEssentials.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsApi.Sample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc(config =>
					{
						config.Conventions.Add(new CommandControllerNameConvention());
					})
				.AddCqrsApi(options =>
					{
                        options.DiscoverAssemblyTypes(typeof(Startup).Assembly);
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
			app.UseMvcWithCqrsApi();
		}
	}
}
