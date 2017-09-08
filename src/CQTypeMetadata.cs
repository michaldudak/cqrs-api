using System;
using System.Net.Http;

namespace CqrsApi
{
	internal class CQTypeMetadata
	{
		public readonly Type CQType;

		public readonly string RouteTemplate;

		public readonly HttpMethod HttpMethod;
	
		public CQTypeMetadata(Type type, string routeTemplate, HttpMethod httpMethod)
		{
			CQType = type ?? throw new ArgumentNullException(nameof(type));
			RouteTemplate = routeTemplate ?? throw new ArgumentNullException(nameof(routeTemplate));
			HttpMethod = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));
		}
	}
}
