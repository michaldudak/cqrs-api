using System;

namespace CqrsApi
{
	internal class CQTypeMetadata
	{
		public readonly Type CQType;

		public readonly string RouteTemplate;

		public CQTypeMetadata(Type type, string routeTemplate)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (routeTemplate == null) throw new ArgumentNullException(nameof(routeTemplate));

			CQType = type;
			RouteTemplate = routeTemplate;
		}
	}
}
