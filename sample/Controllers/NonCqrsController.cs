using Microsoft.AspNetCore.Mvc;

namespace CqrsApi.Sample.Controllers
{
	// This is just another ordinary controller. It should work as if there's no CQRS API. 
	public class NonCqrsController : Controller
	{
		[HttpGet("/non-cqrs-controller")]
		public string Get()
		{
			return "It should just work normally.";
		}
	}
}