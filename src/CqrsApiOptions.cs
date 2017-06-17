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
		internal List<Type> CommandTypes { get; } = new List<Type>();
		internal List<Type> QueryTypes { get; } = new List<Type>();

		public void DiscoverAssemblyTypes(Assembly assembly)
		{
			CommandTypes.AddRange(assembly.GetTypes()
				.Where(c => typeof(ICommand).IsAssignableFrom(c)));

			QueryTypes.AddRange(assembly.GetTypes()
				.Where(c => c.IsClosedTypeOf(typeof(IQuery<>))));
		}
	}

	public class CqrsApiTypeProviderBuilder
	{
		private IEnumerable<CQTypeMetadata> _commandTypes = new List<CQTypeMetadata>();
		private IEnumerable<CQTypeMetadata> _queryTypes = new List<CQTypeMetadata>();

		internal CqrsApiTypeProviderBuilder(IEnumerable<CQTypeMetadata> commandTypes, IEnumerable<CQTypeMetadata> queryTypes)
		{
			_commandTypes = commandTypes;
			_queryTypes = queryTypes;
		}

		private CqrsApiTypeProviderBuilder(CqrsApiTypeProviderBuilder baseBuilder, IEnumerable<CQTypeMetadata> commandTypes, IEnumerable<CQTypeMetadata> queryTypes)
		{
			_commandTypes = baseBuilder._commandTypes.Concat(commandTypes);
			_queryTypes = baseBuilder._commandTypes.Concat(queryTypes);
		}

		public CqrsApiTypeProviderBuilder AddAssemblyTypes(Assembly assembly)
		{
			var assemblyCommands = assembly.GetTypes()
				.Where(c => typeof(ICommand).IsAssignableFrom(c));

			var assemblyQueries = assembly.GetTypes()
				.Where(c => c.IsClosedTypeOf(typeof(IQuery<>)));
			
			return new CqrsApiTypeProviderBuilder(
				this,
				assemblyCommands.Select(t => new CQTypeMetadata(t)),
				assemblyQueries.Select(t => new CQTypeMetadata(t)));
		}

		public CqrsApiTypeProviderBuilder AddCommand(Type type, string routeTemplate = null)
		{
			var typeToAdd = GetTypeMetadada(type, routeTemplate);

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: new[] { typeToAdd },
				queryTypes: Enumerable.Empty<CQTypeMetadata>()
			);
		}

		public CqrsApiTypeProviderBuilder AddQuery(Type type, string routeTemplate = null)
		{
			var typeToAdd = GetTypeMetadada(type, routeTemplate);

			return new CqrsApiTypeProviderBuilder(
				baseBuilder: this,
				commandTypes: Enumerable.Empty<CQTypeMetadata>(),
				queryTypes: new[] { typeToAdd }
			);
		}

		private CQTypeMetadata GetTypeMetadada(Type type, string routeTemplate)
		{
			if (routeTemplate == null)
			{
				return new CQTypeMetadata(type);
			}
			else
			{
				return new CQTypeMetadata(type, routeTemplate);
			}
		}

		public void Build()
		{

		}
	}

	internal class CQTypeMetadata
	{
		public readonly Type CQType;

		public readonly string RouteTemplate;

		public CQTypeMetadata(Type type, string routeTemplate)
		{
			CQType = type;
			RouteTemplate = routeTemplate;
		}

		public CQTypeMetadata(Type type)
		{
			CQType = type;

			var typeName = type.Name;
			var suffixesToRemove = new [] { "Command", "Query" };
			foreach (var suffix in suffixesToRemove)
			{
				if (typeName.EndsWith(suffix))
				{
					var suffixStartPosition = typeName.Length - suffix.Length;
					typeName = typeName.Substring(0, suffixStartPosition);
					break;
				}
			}

			RouteTemplate = typeName;
		}
	}
}