using System;
using System.Linq;
using CqrsEssentials;

namespace CqrsApi.Tests.TestApp
{
	public class GetNumbersQuery : IQuery<int[]>
	{
		public int Count { get; set; }
	}

	public class GetNumbersQueryHandler : IQueryHandler<GetNumbersQuery, int[]>
	{
		public int[] Handle(GetNumbersQuery query)
		{
			return Enumerable.Range(1, query.Count).ToArray();
		}
	}
}
