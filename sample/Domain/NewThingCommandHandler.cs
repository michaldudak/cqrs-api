using CqrsApi.Sample.CommandsAndQueries;
using CqrsEssentials;

namespace CqrsApi.Sample.Domain
{
	public class NewThingCommandHandler : ICommandHandler<NewThingCommand>
	{
		public void Handle(NewThingCommand command)
		{
			Things.Items.Add(command.Name);
		}
	}
}