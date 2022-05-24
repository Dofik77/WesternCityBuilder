using System;

namespace PdUtils.FirstStartService
{
	public interface IFirstStartService
	{
		void SaveFirstStart(Action firstStartAction);
		DateTime? GetFirstStartTimeUtc();
	}
}