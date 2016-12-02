using System.Collections.Generic;
using System.Reflection;
using CqrsApi.Web.CommandsAndQueries;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CqrsApi.Web.Infrastructure
{
	public class CommandControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			var types = new[] { typeof(NewThingCommand) };

			foreach (var type in types)
			{
				var controllerType = typeof(CommandController<>).MakeGenericType(type).GetTypeInfo();
				feature.Controllers.Add(controllerType);
			}
		}
	}
}