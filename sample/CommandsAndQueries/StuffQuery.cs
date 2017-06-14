using System.Collections.Generic;
using CqrsEssentials;

namespace CqrsApi.Sample.CommandsAndQueries
{
	public class RandomStuffQuery : IQuery<ICollection<string>>
	{}
}