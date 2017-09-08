using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Autofac;

namespace CqrsApi
{
	public class CqrsApiControllerMetadataConvention : Attribute, IControllerModelConvention
	{
		// This will change the controller name from something like 'CommandController`1' into the proper name.
		// More importantly, it'll also create the route under which the controller is accessible.
		public void Apply(ControllerModel controller)
		{
			var isCommandController = controller.ControllerType.IsClosedTypeOf(typeof(CommandController<>));
			var isQueryController = !isCommandController && controller.ControllerType.IsClosedTypeOf(typeof(QueryController<,>));

			if (!isCommandController && !isQueryController)
			{
				// Just an ordinary controller, ignore it.
				return;
			}

			var entityType = controller.ControllerType.GenericTypeArguments[0];
			controller.ControllerName = entityType.Name;
			controller.ApiExplorer.IsVisible = true;

			// TODO: make sure there is a single selector (what if there are custom routes defined by the user?)
			controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
			{
				// TODO: now that it's proven it works, let's make the route customizable
				Template = entityType.Name.Replace(isCommandController ? "Command" : "Query", string.Empty)
			};

			controller.Selectors[0].ActionConstraints.Add(new HttpMethodActionConstraint(isCommandController ? new [] { "POST" } : new [] { "GET" }));

			// TODO: allow customizations (through attributes?), like caching, etc.
			// Authz shouldn't be neccessary as it should be the command handler's responsibility.
		}
	}
}
