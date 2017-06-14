using System;
using System.Collections.Generic;
using CqrsApi.Sample.CommandsAndQueries;
using CqrsEssentials;

namespace CqrsApi.Sample.Domain
{
	public class RandomStuffQueryHandler : IQueryHandler<RandomStuffQuery, ICollection<string>>
	{
		public ICollection<string> Handle(RandomStuffQuery query)
		{
			var random = new Random();

			var array = new string[random.Next(3, 10)];

			for (var i = 0; i < array.Length; ++i)
			{
				array[i] = "Item " + random.Next();
			}

			return array;
		}
	}
}
