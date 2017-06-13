using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CqrsApi
{
	public class CommandControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		private readonly CqrsApiOptions _cqrsApiOptions;

		public CommandControllerFeatureProvider(CqrsApiOptions cqrsApiOptions)
		{
			_cqrsApiOptions = cqrsApiOptions;
		}

		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			var types = _cqrsApiOptions.Configs.Where(c => typeof(ICommand).IsAssignableFrom(c.InputType)).Select(c => c.InputType);

			foreach (var type in types)
			{
				var controllerType = typeof(CommandController<>).MakeGenericType(type).GetTypeInfo();
				feature.Controllers.Add(controllerType);
			}
		}
	}
}