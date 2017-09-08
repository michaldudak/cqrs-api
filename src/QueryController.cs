using System.Threading.Tasks;
using CqrsEssentials;
using Microsoft.AspNetCore.Mvc;

namespace CqrsApi
{
	[NonController]
	public class QueryController<TQuery, TResult> where TQuery : IQuery<TResult>
	{
		private readonly IQueryDispatcher _queryDispatcher;

		public QueryController(IQueryDispatcher queryDispatcher)
		{
			_queryDispatcher = queryDispatcher;
		}

		public async Task<TResult> Index([FromQuery]TQuery query)
		{
			return await _queryDispatcher.DispatchAsync(query);
		}
	}
}
