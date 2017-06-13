using System.Threading.Tasks;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc;

namespace CqrsApi
{
	[CommandControllerNameConvention]
	public class CommandController<TCommand> : Controller where TCommand : class, ICommand
	{
		private readonly ICommandDispatcher _commandDispatcher;

		public CommandController(ICommandDispatcher commandDispatcher)
		{
			_commandDispatcher = commandDispatcher;
		}

		public async Task Index([FromBody]TCommand command)
		{
			await _commandDispatcher.DispatchAsync(command);
		}
	}
}