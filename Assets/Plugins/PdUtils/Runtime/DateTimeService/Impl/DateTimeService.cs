using System;

namespace PdUtils.DateTimeService.Impl
{
	public class DateTimeService : IDateTimeService
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
		public DateTimeOffset Now => DateTimeOffset.Now;
	}
}