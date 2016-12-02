using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CqrsApi.Web.CommandsAndQueries;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CqrsApi.Web.Infrastructure
{
	public class QueryControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			var types = new[] { typeof(ThingsQuery) };

			foreach (var type in types)
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