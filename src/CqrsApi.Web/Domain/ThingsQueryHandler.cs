using System.Collections.Generic;
using System.Linq;
using CqrsApi.Web.CommandsAndQueries;
using CqrsEssentials;

namespace CqrsApi.Web.Domain
{
	public class ThingsQueryHandler : IQueryHandler<ThingsQuery, ICollection<Thing>>
	{
		public ICollection<Thing> Handle(ThingsQuery query)
		{
			return Things.Items.Where(t => t.StartsWith(query.NamePrefix)).Select(t => new Thing(t)).ToList();
		}
	}
}
