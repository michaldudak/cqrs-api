using System.Threading.Tasks;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc;

namespace CqrsApi.Web.Infrastructure
{
	[CommandControllerNameConvention]
	public class CommandController<TCommand> where TCommand : ICommand
	{
		private readonly ICommandDispatcher _commandDispatcher;

		public CommandController(ICommandDispatcher commandDispatcher)
		{
			_commandDispatcher = commandDispatcher;
		}

		[HttpPost]
		public async Task Index([FromBody]TCommand command)
		{
			await _commandDispatcher.DispatchAsync(command);
		}
	}
}