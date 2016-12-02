using System.Collections.Generic;
using CqrsApi.Web.Domain;
using CqrsEssentials;

namespace CqrsApi.Web.CommandsAndQueries
{
	public class ThingsQuery : IQuery<ICollection<Thing>>
	{
		public string NamePrefix { get; set; }
	}
}