using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CqrsEssentials.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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
				.AddMvcWithCqrsApi(cqrsApiSetupAction: options =>
					{
						options.Builder
							// The following line will add all the assembly's queries with customized URLs
							.AddAssemblyQueries(typeof(Startup).Assembly, typeName => "Get" + typeName.Replace("Query", ""))
							// Commands are registered with default URLs (that is type name without the 'Command' suffix
							.AddAssemblyCommands(typeof(Startup).Assembly)
							.Build();
					});

			services.AddSwaggerGen(options =>
				options.SwaggerDoc("v1", new Info { Title = "CQRS API Sample", Version = "v1" })
			);

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

			// TODO: make Swagger work
			app.UseSwagger();
			app.UseSwaggerUI(options =>
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "CQRS API Sample")
			);
		}
	}
}
