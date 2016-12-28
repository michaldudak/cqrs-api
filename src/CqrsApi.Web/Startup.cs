using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CqrsApi.Web.CommandsAndQueries;
using CqrsApi.Web.Infrastructure;
using CqrsEssentials.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CqrsApi.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc(config =>
					{
						config.Conventions.Add(new CommandControllerNameConvention());
					})
				.AddCqrsApi(options =>
					{
						options.MapGet("things").ToQuery<ThingsQuery>();
						options.MapPost("new-thing").ToCommand<NewThingCommand>();
					});

			var builder = new ContainerBuilder();

			builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsImplementedInterfaces();
			builder.RegisterModule<CqrsEssentialsAutofacModule>();
			builder.Populate(services);
			var container = builder.Build();

			return new AutofacServiceProvider(container);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseMvcWithCqrsApi();
		}
	}
}
