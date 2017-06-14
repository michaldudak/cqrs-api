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
}