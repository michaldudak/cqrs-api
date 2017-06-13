namespace CqrsApi.Sample.Domain
{
	public class Thing
	{
		public Thing(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}