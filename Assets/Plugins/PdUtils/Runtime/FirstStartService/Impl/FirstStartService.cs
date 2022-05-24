using System;
using System.Globalization;
using PdUtils.DateTimeService;
using PdUtils.PlayerPrefs;

namespace PdUtils.FirstStartService.Impl
{
	public class FirstStartService : IFirstStartService
	{
		private const string FirstStartKey = "Game.FirstStart";
		private const string FirstStartTimeKey = "Game.FirstStartTime";

		private readonly IPlayerPrefsManager _prefsManager;
		private readonly IDateTimeService _dateTimeService;

		private DateTime? _firstTimeUtc;

		public FirstStartService(IPlayerPrefsManager prefsManager, IDateTimeService dateTimeService)
		{
			_prefsManager = prefsManager;
			_dateTimeService = dateTimeService;
		}

		public void SaveFirstStart(Action firstStartAction)
		{
			if (!_prefsManager.HasKey(FirstStartKey))
			{
				_prefsManager.SetValue(FirstStartKey, true);
				firstStartAction?.Invoke();

				var startTimeStr = _dateTimeService.UtcNow.DateTime.ToString("G");
				_prefsManager.SetValue(FirstStartTimeKey, startTimeStr);
			}
			else
			{
				_prefsManager.SetValue(FirstStartKey, false);
			}
			_prefsManager.Save();
		}

		public DateTime? GetFirstStartTimeUtc()
		{
			if (!_prefsManager.HasKey(FirstStartTimeKey))
				return null;

			if (_firstTimeUtc.HasValue)
				return _firstTimeUtc.Value;
			
			var startTimeStr = _prefsManager.GetValue<string>(FirstStartTimeKey);
			_firstTimeUtc = DateTime.Parse(startTimeStr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
			return _firstTimeUtc;
		}
	}
}