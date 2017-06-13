using System.Collections.Generic;
using CqrsApi.Sample.Domain;
using CqrsEssentials;

namespace CqrsApi.Sample.CommandsAndQueries
{
	public class ThingsQuery : IQuery<ICollection<Thing>>
	{
		public string NamePrefix { get; set; }
	}
}