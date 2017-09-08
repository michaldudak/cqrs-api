using System.Collections.Generic;
using System.Linq;

namespace CqrsApi
{
	public class CqrsApiOptions
	{
		internal IEnumerable<CQTypeMetadata> CommandTypes { get; set; } = Enumerable.Empty<CQTypeMetadata>();
		internal IEnumerable<CQTypeMetadata> QueryTypes { get; set; } = Enumerable.Empty<CQTypeMetadata>();

		public CqrsApiTypeProviderBuilder Builder { get; }

		public CqrsApiOptions()
		{
			Builder = new CqrsApiTypeProviderBuilder(this);
		}
	}
}
