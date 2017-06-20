using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CqrsApi
{
	public class QueryControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		private readonly CqrsApiOptions _apiOptions;

		public QueryControllerFeatureProvider(CqrsApiOptions apiOptions)
		{
			_apiOptions = apiOptions;
		}

		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			foreach (var type in _apiOptions.QueryTypes.Select(qt => qt.CQType))
			{
				var returnType =
					type.GetInterfaces()
						.Where(i => i.GetGenericTypeDefinition() == typeof(IQuery<>))
						.Select(i => i.GenericTypeArguments.First())
						.Single();

				var controllerType = typeof(QueryController<,>).MakeGenericType(type, returnType).GetTypeInfo();
				feature.Controllers.Add(controllerType);
			}
		}
	}
}