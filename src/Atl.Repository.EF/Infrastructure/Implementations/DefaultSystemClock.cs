using System;
using Atl.Repository.Standard.Infrastructure.Contracts;

namespace Atl.Repository.EF.Infrastructure.Implementations
{
	public class DefaultSystemClock : IClock
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
