using CqrsApi.Web.CommandsAndQueries;
using CqrsEssentials;

namespace CqrsApi.Web.Domain
{
	public class NewThingCommandHandler : ICommandHandler<NewThingCommand>
	{
		public void Handle(NewThingCommand command)
		{
			Things.Items.Add(command.Name);
		}
	}
}