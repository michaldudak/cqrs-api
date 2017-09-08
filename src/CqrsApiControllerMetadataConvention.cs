using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Autofac;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CqrsApi
{
	public class CqrsApiControllerMetadataConvention : IControllerModelConvention, IFilterMetadata
	{
		private readonly CqrsApiOptions _cqrsApiOptions;

		public CqrsApiControllerMetadataConvention(CqrsApiOptions cqrsApiOptions)
		{
			_cqrsApiOptions = cqrsApiOptions;
		}

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

			CQTypeMetadata configuration = null;
			if (isCommandController)
			{
				configuration = _cqrsApiOptions.CommandTypes.SingleOrDefault(t => t.CQType == entityType);
			} else if (isQueryController)
			{
				configuration = _cqrsApiOptions.QueryTypes.SingleOrDefault(t => t.CQType == entityType);
			}

			if (configuration == null)
			{
				throw new InternalException("Entity type not found in the configuration.");
			}

			// TODO: make sure there is a single selector (what if there are custom routes defined by the user?)
			controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
			{
				Template = configuration.RouteTemplate
			};

			controller.Selectors[0].ActionConstraints.Add(new HttpMethodActionConstraint(new [] { configuration.HttpMethod.Method }));

			// TODO: allow customizations (through attributes?), like caching, etc.
			// Authz shouldn't be neccessary as it should be the command handler's responsibility.
		}
	}
}
