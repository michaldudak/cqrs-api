using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Autofac;
using CqrsEssentials;

namespace CqrsApi
{
	public class CqrsApiOptions
	{
		internal IEnumerable<CQTypeMetadata> CommandTypes { get; set; } = Enumerable.Empty<CQTypeMetadata>();
		internal IEnumerable<CQTypeMetadata> QueryTypes { get; set; } = Enumerable.Empty<CQTypeMetadata>();

		public CqrsApiTypeProviderBuilder Builder { get; }

		public CqrsApiOptions()
		{
			Builder = new CqrsApiTypeProviderBuilder(this);
		}
	}

	public class CqrsApiTypeProviderBuilder
	{
		private static readonly string[] _typeSuffixesToRemove = new[] { "Command", "Query" };

		private IEnumerable<CQTypeMetadata> _commandTypes = new List<CQTypeMetadata>();
		private IEnumerable<CQTypeMetadata> _queryTypes = new List<CQTypeMetadata>();

		private readonly CqrsApiOptions _cqrsApiOptions;

		internal CqrsApiTypeProviderBuilder(CqrsApiOptions cqrsApiOptions)
		{
			_cqrsApiOptions = cqrsApiOptions;
		}

		private CqrsApiTypeProviderBuilder(CqrsApiTypeProviderBuilder baseBuilder, IEnumerable<CQTypeMetadata> commandTypes, IEnumerable<CQTypeMetadata> queryTypes)
		{
			_cqrsApiOptions = baseBuilder._cqrsApiOptions;
			_commandTypes = baseBuilder._commandTypes.Concat(commandTypes);
			_queryTypes = baseBuilder._commandTypes.Concat(queryTypes);
		}

		public CqrsApiTypeProviderBuilder AddAssemblyTypes(Assembly assembly, Func<string, string> routeTemplateTransformFn = null)
		{
			if (routeTemplateTransformFn == null)
			{
				routeTemplateTransformFn = DefaultRouteTemplateTransformFunction;
			}

			var assemblyCommands = assembly.GetTypes()
				.Where(c => typeof(ICommand).IsAssignableFrom(c));

			var assemblyQueries = assembly.GetTypes()
				.Where(c => c.IsClosedTypeOf(typeof(IQuery<>)));

			return new CqrsApiTypeProviderBuilder(
				this,
				assemblyCommands.Select(t => new CQTypeMetadata(t, routeTemplateTransformFn(t.Name))),
				assemblyQueries.Select(t => new CQTypeMetadata(t, routeTemplateTransformFn(t.Name))));
		}

		public CqrsApiTypeProviderBuilder AddCommand(Type type, string routeTemplate = null)
		{
			var typeToAdd = new CQTypeMetadata(type, routeTemplate ?? DefaultRouteTemplateTransformFunction(type.Name));

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: new[] { typeToAdd },
				queryTypes: Enumerable.Empty<CQTypeMetadata>()
			);
		}

		public CqrsApiTypeProviderBuilder AddQuery(Type type, string routeTemplate = null)
		{
			var typeToAdd = new CQTypeMetadata(type, routeTemplate ?? DefaultRouteTemplateTransformFunction(type.Name));

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: Enumerable.Empty<CQTypeMetadata>(),
				queryTypes: new[] { typeToAdd }
			);
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

	internal class CQTypeMetadata
	{
		public readonly Type CQType;

		public readonly string RouteTemplate;

		public CQTypeMetadata(Type type, string routeTemplate)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (routeTemplate == null) throw new ArgumentNullException(nameof(routeTemplate));

			CQType = type;
			RouteTemplate = routeTemplate;
		}
	}
}