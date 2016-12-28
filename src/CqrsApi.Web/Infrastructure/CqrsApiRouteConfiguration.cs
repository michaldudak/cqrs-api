using System;
using CqrsEssentials;

namespace CqrsApi.Web.Infrastructure
{
	public class CqrsApiRouteConfiguration
	{
		public CqrsApiRouteConfiguration(string verb, string urlTemplate)
		{
			Verb = verb;
			UrlTemplate = urlTemplate;
		}

		public void ToCommand<TCommand>() where TCommand : ICommand
		{
			InputType = typeof(TCommand);
		}

		public void ToQuery<TQuery>()
		{
			InputType = typeof(TQuery);
		}

		internal string Verb { get; }

		internal string UrlTemplate { get; }

		internal Type InputType { get; private set; }
	}
}