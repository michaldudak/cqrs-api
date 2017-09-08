using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsApi
{
	internal class InternalException : Exception
	{
		public InternalException(string message) : base(message)
		{
		}
	}
}
