using System.Collections.Generic;
using System.Linq;
using CqrsApi.Sample.CommandsAndQueries;
using CqrsEssentials;

namespace CqrsApi.Sample.Domain
{
	public class ThingsQueryHandler : IQueryHandler<ThingsQuery, ICollection<Thing>>
	{
		public ICollection<Thing> Handle(ThingsQuery query)
		{
			var namePrefix = query.NamePrefix ?? "";
			return Things.Items.Where(t => t.StartsWith(namePrefix)).Select(t => new Thing(t)).ToList();
		}
	}
}
