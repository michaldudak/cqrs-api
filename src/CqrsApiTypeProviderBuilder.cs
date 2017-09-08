using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Autofac;
using CqrsEssentials;

namespace CqrsApi
{
	public class CqrsApiTypeProviderBuilder
	{
		private static readonly string[] _typeSuffixesToRemove = { "Command", "Query" };

		private readonly IEnumerable<CQTypeMetadata> _commandTypes = new List<CQTypeMetadata>();
		private readonly IEnumerable<CQTypeMetadata> _queryTypes = new List<CQTypeMetadata>();

		private readonly CqrsApiOptions _cqrsApiOptions;

		internal CqrsApiTypeProviderBuilder(CqrsApiOptions cqrsApiOptions)
		{
			_cqrsApiOptions = cqrsApiOptions;
		}

		private CqrsApiTypeProviderBuilder(CqrsApiTypeProviderBuilder baseBuilder, IEnumerable<CQTypeMetadata> commandTypes, IEnumerable<CQTypeMetadata> queryTypes)
		{
			_cqrsApiOptions = baseBuilder._cqrsApiOptions;
			_commandTypes = baseBuilder._commandTypes.Concat(commandTypes);
			_queryTypes = baseBuilder._queryTypes.Concat(queryTypes);
		}

		public CqrsApiTypeProviderBuilder AddAssemblyTypes(Assembly assembly, Func<string, string> routeTemplateTransformFn = null)
		{
			return AddAssemblyCommands(assembly, routeTemplateTransformFn)
				.AddAssemblyQueries(assembly, routeTemplateTransformFn);
		}

		public CqrsApiTypeProviderBuilder AddAssemblyCommands(Assembly assembly, Func<string, string> routeTemplateTransformFn = null)
		{
			if (routeTemplateTransformFn == null)
			{
				routeTemplateTransformFn = DefaultRouteTemplateTransformFunction;
			}

			var assemblyCommands = assembly.GetTypes()
				.Where(c => typeof(ICommand).IsAssignableFrom(c));

			return new CqrsApiTypeProviderBuilder(
				this,
				assemblyCommands.Select(t => new CQTypeMetadata(t, routeTemplateTransformFn(t.Name), HttpMethod.Post)),
				Enumerable.Empty<CQTypeMetadata>());
		}

		public CqrsApiTypeProviderBuilder AddAssemblyQueries(Assembly assembly, Func<string, string> routeTemplateTransformFn = null)
		{
			if (routeTemplateTransformFn == null)
			{
				routeTemplateTransformFn = DefaultRouteTemplateTransformFunction;
			}

			var assemblyQueries = assembly.GetTypes()
				.Where(c => c.IsClosedTypeOf(typeof(IQuery<>)));

			return new CqrsApiTypeProviderBuilder(
				this,
				Enumerable.Empty<CQTypeMetadata>(),
				assemblyQueries.Select(t => new CQTypeMetadata(t, routeTemplateTransformFn(t.Name), HttpMethod.Get)));
		}

		public CqrsApiTypeProviderBuilder AddCommand(Type type, string routeTemplate = null)
		{
			var typeToAdd = new CQTypeMetadata(type, routeTemplate ?? DefaultRouteTemplateTransformFunction(type.Name), HttpMethod.Post);

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: new[] { typeToAdd },
				queryTypes: Enumerable.Empty<CQTypeMetadata>()
			);
		}

		public CqrsApiTypeProviderBuilder AddCommand<T>(string routeTemplate = null)
		{
			return AddCommand(typeof(T), routeTemplate);
		}

		public CqrsApiTypeProviderBuilder AddQuery(Type type, string routeTemplate = null)
		{
			var typeToAdd = new CQTypeMetadata(type, routeTemplate ?? DefaultRouteTemplateTransformFunction(type.Name), HttpMethod.Get);

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: Enumerable.Empty<CQTypeMetadata>(),
				queryTypes: new[] { typeToAdd }
			);
		}

		public CqrsApiTypeProviderBuilder AddQuery<T>(string routeTemplate = null)
		{
			return AddQuery(typeof(T), routeTemplate);
		}

		public void Build()
		{
			_cqrsApiOptions.CommandTypes = _commandTypes;
			_cqrsApiOptions.QueryTypes = _queryTypes;
		}

		private static string DefaultRouteTemplateTransformFunction(string typeName)
		{
			for (var i = 0; i < _typeSuffixesToRemove.Length; ++i)
			{
				var suffix = _typeSuffixesToRemove[i];
				if (typeName.EndsWith(suffix))
				{
					var suffixStartPosition = typeName.Length - suffix.Length;
					typeName = typeName.Substring(0, suffixStartPosition);
					break;
				}
			}

			return typeName;
		}
	}
}
