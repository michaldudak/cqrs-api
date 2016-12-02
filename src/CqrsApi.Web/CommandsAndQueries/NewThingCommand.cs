using CqrsEssentials;

namespace CqrsApi.Web.CommandsAndQueries
{
	public class NewThingCommand : ICommand
	{
		public string Name { get; set; }
	}
}