using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CqrsApi
{
	public class CommandControllerNameConvention : Attribute, IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			if (controller.ControllerType.GetGenericTypeDefinition() != typeof(CommandController<>))
			{
				// Not a CommandController, ignore.
				return;
			}

			var entityType = controller.ControllerType.GenericTypeArguments[0];
			controller.ControllerName = entityType.Name;
		}
	}
}