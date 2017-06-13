using CqrsEssentials;

namespace CqrsApi.Sample.CommandsAndQueries
{
	public class NewThingCommand : ICommand
	{
		public string Name { get; set; }
	}
}