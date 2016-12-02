using System;
using CqrsEssentials;

namespace CqrsApi.Web.Infrastructure
{
	public class CqrsApiRouteConfiguration
	{
		private readonly string _verb;
		private readonly string _urlTemplate;
		private Type _inputType;

		// TODO Implement Autofac-like builder

		public CqrsApiRouteConfiguration(string verb, string urlTemplate)
		{
			_verb = verb;
			_urlTemplate = urlTemplate;
		}

		public void ToCommand<TCommand>() where TCommand : ICommand
		{
			_inputType = typeof(TCommand);
		}

		public void ToQuery<TQuery>()
		{
			_inputType = typeof(TQuery);
		}
	}
}