using System;

namespace PdUtils.DateTimeService
{
	public interface IDateTimeService
	{
		DateTimeOffset UtcNow { get; }
		DateTimeOffset Now { get; }
	}
}