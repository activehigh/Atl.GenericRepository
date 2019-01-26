using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Internal;

namespace Atl.Repository.Standard.Infrastructure.Implementations
{
	public class DefaultSystemClock : ISystemClock
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
