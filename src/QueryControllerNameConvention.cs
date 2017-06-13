using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CqrsApi
{
	public class QueryControllerNameConvention : Attribute, IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			if (controller.ControllerType.GetGenericTypeDefinition() != typeof(QueryController<,>))
			{
				// Not a QueryController, ignore.
				return;
			}

			var entityType = controller.ControllerType.GenericTypeArguments[0];
			controller.ControllerName = entityType.Name;
		}
	}
}